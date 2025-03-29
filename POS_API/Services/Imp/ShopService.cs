using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Shared.DTOs;
using POS.Shared;
using POS.Shared.Entities;
using POS_API.Repository.IRepository;
using POS_API.Services.IServices;

namespace POS_API.Services.Imp
{
    public class ShopService : IShopService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShopService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<ShopDto>> GetShopsAsync(ShopSearchDto searchDto)
        {
            var shops = await _unitOfWork.Shops.GetAllAsync();
            var query = shops.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchDto.Name))
            {
                query = query.Where(s => s.Name.Contains(searchDto.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(searchDto.Address))
            {
                query = query.Where(s => s.Address.Contains(searchDto.Address, StringComparison.OrdinalIgnoreCase));
            }

            // Get total count
            var totalCount = query.Count();

            // Apply sorting
            if (!string.IsNullOrEmpty(searchDto.SortBy))
            {
                switch (searchDto.SortBy.ToLower())
                {
                    case "name":
                        query = searchDto.IsDescending
                            ? query.OrderByDescending(s => s.Name)
                            : query.OrderBy(s => s.Name);
                        break;
                    case "address":
                        query = searchDto.IsDescending
                            ? query.OrderByDescending(s => s.Address)
                            : query.OrderBy(s => s.Address);
                        break;
                    default:
                        query = searchDto.IsDescending
                            ? query.OrderByDescending(s => s.Id)
                            : query.OrderBy(s => s.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(s => s.Id);
            }

            // Apply pagination
            var pagedShops = query
                .Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToList();

            var shopDtos = _mapper.Map<List<ShopDto>>(pagedShops);

            return new PagedResultDto<ShopDto>(shopDtos, totalCount, searchDto.PageNumber, searchDto.PageSize);
        }

        public async Task<ShopDto> GetShopByIdAsync(int id)
        {
            var shop = (await _unitOfWork.Shops.FindAsync(s => s.Id == id)).FirstOrDefault();
            if (shop == null)
            {
                return null;
            }

            return _mapper.Map<ShopDto>(shop);
        }

        public async Task<ShopDto> CreateShopAsync(CreateShopDto createShopDto)
        {
            var shop = _mapper.Map<Shop>(createShopDto);

            await _unitOfWork.Shops.AddAsync(shop);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ShopDto>(shop);
        }

        public async Task<bool> UpdateShopAsync(int id, UpdateShopDto updateShopDto)
        {
            var shop = (await _unitOfWork.Shops.FindAsync(s => s.Id == id)).FirstOrDefault();
            if (shop == null)
            {
                return false;
            }

            // Update shop properties
            shop.Name = updateShopDto.Name;
            shop.Address = updateShopDto.Address;

            _unitOfWork.Shops.Update(shop);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteShopAsync(int id)
        {
            var shop = (await _unitOfWork.Shops.FindAsync(s => s.Id == id)).FirstOrDefault();
            if (shop == null)
            {
                return false;
            }

            // Check if shop has any products or orders
            if (shop.Products.Any() || shop.Orders.Any())
            {
                throw new InvalidOperationException("Không thể xóa cửa hàng đã có sản phẩm hoặc đơn hàng");
            }

            _unitOfWork.Shops.Remove(shop);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
