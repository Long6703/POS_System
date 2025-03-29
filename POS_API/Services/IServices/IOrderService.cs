using POS.Shared.DTOs.Order;
using POS.Shared;

namespace POS_API.Services.IServices
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO createOrderDTO);
        Task<OrderResponseDTO> UpdateOrderAsync(UpdateOrderDTO updateOrderDTO);
        Task<PaymentResponseDTO> ConfirmPaymentAsync(CreatePaymentDTO createPaymentDTO);
        Task<PagedResultDto<OrderResponseDTO>> GetStaffOrdersAsync(OrderSearchDTO searchDTO);
        Task<OrderResponseDTO> GetOrderByIdAsync(Guid id);
    }
}
