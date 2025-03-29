using POS.Shared.DTOs;
using POS.Shared;

namespace POS_API.Services.IServices
{
    public interface IShopService
    {
        Task<PagedResultDto<ShopDto>> GetShopsAsync(ShopSearchDto searchDto);
        Task<ShopDto> GetShopByIdAsync(int id);
        Task<bool> CreateShopAsync(CreateShopDto createShopDto);
        Task<bool> UpdateShopAsync(int id, UpdateShopDto updateShopDto);
        Task<bool> DeleteShopAsync(int id);
    }
}
