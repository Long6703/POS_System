using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Shared.DTOs.Order;
using POS.Shared;
using POS.Shared.Entities;
using POS_API.Repository.IRepository;
using POS_API.Services.IServices;
using System.Security.Claims;
using static POS_API.Enum.Enums;

namespace POS_API.Services.Imp
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated or user ID is invalid");
            }
            return userId;
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO createOrderDTO)
        {
            var userId = GetCurrentUserId();

            // Verify shop exists
            var shop = await _unitOfWork.Shops.GetByIdAsync(createOrderDTO.ShopId);
            if (shop == null)
                throw new KeyNotFoundException($"Shop with ID {createOrderDTO.ShopId} not found");

            // Create order
            var order = _mapper.Map<Order>(createOrderDTO);
            order.Id = Guid.NewGuid();

            // Process order details
            decimal totalAmount = 0;
            foreach (var item in createOrderDTO.OrderDetails)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product with ID {item.ProductId} not found");

                if (product.Stock < item.Quantity)
                    throw new InvalidOperationException($"Not enough stock for product {product.Name}");

                var orderDetail = _mapper.Map<OrderDetail>(item);
                orderDetail.OrderId = order.Id;
                orderDetail.Price = product.Price;

                totalAmount += orderDetail.Price * orderDetail.Quantity;

                // Update product stock
                product.Stock -= item.Quantity;
                _unitOfWork.Products.Update(product);

                await _unitOfWork.OrderDetails.AddAsync(orderDetail);
            }

            order.TotalAmount = totalAmount;

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            return await GetOrderByIdAsync(order.Id);
        }

        public async Task<OrderResponseDTO> UpdateOrderAsync(UpdateOrderDTO updateOrderDTO)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(updateOrderDTO.Id);
            if (order == null)
                return null;

            // Update basic order information
            order.CustomerEmail = updateOrderDTO.CustomerEmail;
            order.CustomerPhoneNumber = updateOrderDTO.CustomerPhoneNumber;
            order.Status = (OrderStatus)updateOrderDTO.Status;

            // Handle order details if provided
            if (updateOrderDTO.OrderDetails != null && updateOrderDTO.OrderDetails.Any())
            {
                // Get existing order details
                var existingDetails = (await _unitOfWork.OrderDetails.FindAsync(od => od.OrderId == order.Id)).ToList();

                // Remove all existing details
                _unitOfWork.OrderDetails.RemoveRange(existingDetails);

                // Add new details
                decimal totalAmount = 0;
                foreach (var item in updateOrderDTO.OrderDetails)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    if (product == null)
                        throw new KeyNotFoundException($"Product with ID {item.ProductId} not found");

                    // Calculate how many items to add/remove from stock
                    var existingDetail = existingDetails.FirstOrDefault(ed => ed.ProductId == item.ProductId);
                    int stockChange = existingDetail != null ? existingDetail.Quantity - item.Quantity : -item.Quantity;

                    if (product.Stock + stockChange < 0)
                        throw new InvalidOperationException($"Not enough stock for product {product.Name}");

                    // Update product stock
                    product.Stock += stockChange;
                    _unitOfWork.Products.Update(product);

                    var orderDetail = _mapper.Map<OrderDetail>(item);
                    orderDetail.Id = Guid.NewGuid();
                    orderDetail.OrderId = order.Id;
                    orderDetail.Price = product.Price;

                    totalAmount += orderDetail.Price * orderDetail.Quantity;

                    await _unitOfWork.OrderDetails.AddAsync(orderDetail);
                }

                order.TotalAmount = totalAmount;
            }

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync();

            return await GetOrderByIdAsync(order.Id);
        }

        public async Task<PaymentResponseDTO> ConfirmPaymentAsync(CreatePaymentDTO createPaymentDTO)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(createPaymentDTO.OrderId);
            if (order == null)
                return null;

            // Create payment record
            var payment = _mapper.Map<Payment>(createPaymentDTO);

            // Update order status to paid
            order.Status = OrderStatus.Paid;
            _unitOfWork.Orders.Update(order);

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PaymentResponseDTO>(payment);
        }

        public async Task<PagedResultDto<OrderResponseDTO>> GetStaffOrdersAsync(OrderSearchDTO searchDTO)
        {
            var userId = GetCurrentUserId();

            // Get orders with filtering
            var query = (await _unitOfWork.Orders.FindAsync(o =>
                (searchDTO.ShopId == 0 || o.ShopId == searchDTO.ShopId) &&
                (string.IsNullOrEmpty(searchDTO.CustomerEmail) || o.CustomerEmail.Contains(searchDTO.CustomerEmail)) &&
                (string.IsNullOrEmpty(searchDTO.CustomerPhoneNumber) || o.CustomerPhoneNumber.Contains(searchDTO.CustomerPhoneNumber)) &&
                (!searchDTO.Status.HasValue || o.Status == (OrderStatus)searchDTO.Status) &&
                (!searchDTO.FromDate.HasValue || o.CreatedAt >= searchDTO.FromDate) &&
                (!searchDTO.ToDate.HasValue || o.CreatedAt <= searchDTO.ToDate)
            )).ToList();

            // Apply pagination
            var totalCount = query.Count;
            var items = query
                .Skip((searchDTO.PageNumber - 1) * searchDTO.PageSize)
                .Take(searchDTO.PageSize)
                .ToList();

            // Load related data for each order
            var detailedItems = new List<OrderResponseDTO>();
            foreach (var order in items)
            {
                var orderDetails = await _unitOfWork.OrderDetails.FindAsync(od => od.OrderId == order.Id);
                var payments = await _unitOfWork.Payments.FindAsync(p => p.OrderId == order.Id);

                // Load shop and products
                order.Shop = await _unitOfWork.Shops.GetByIdAsync(order.ShopId);
                foreach (var detail in orderDetails)
                {
                    detail.Product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                }

                var orderResponseDTO = _mapper.Map<OrderResponseDTO>(order);
                orderResponseDTO.OrderDetails = _mapper.Map<List<OrderDetailResponseDTO>>(orderDetails.ToList());
                orderResponseDTO.Payments = _mapper.Map<List<PaymentResponseDTO>>(payments.ToList());

                detailedItems.Add(orderResponseDTO);
            }

            return new PagedResultDto<OrderResponseDTO>(
                detailedItems,
                totalCount,
                searchDTO.PageNumber,
                searchDTO.PageSize
            );
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(Guid id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return null;

            // Load related data
            var orderDetails = await _unitOfWork.OrderDetails.FindAsync(od => od.OrderId == id);
            var payments = await _unitOfWork.Payments.FindAsync(p => p.OrderId == id);

            // Load shop and products
            order.Shop = await _unitOfWork.Shops.GetByIdAsync(order.ShopId);
            foreach (var detail in orderDetails)
            {
                detail.Product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
            }

            var orderResponseDTO = _mapper.Map<OrderResponseDTO>(order);
            orderResponseDTO.OrderDetails = _mapper.Map<List<OrderDetailResponseDTO>>(orderDetails.ToList());
            orderResponseDTO.Payments = _mapper.Map<List<PaymentResponseDTO>>(payments.ToList());

            return orderResponseDTO;
        }
    }
}
