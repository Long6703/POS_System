using POS.Shared.DTOs;
using POS.Shared;
using static POS.Shared.PagedResultDto<T>;

namespace POS_API.Services.IServices
{
    public interface IShopService
    {
        Task<PagedResultDto<ShopDto>> GetShopsAsync(ShopSearchDto searchDto);
        Task<ShopDto> GetShopByIdAsync(int id);
        Task<ShopDto> CreateShopAsync(CreateShopDto createShopDto);
        Task<bool> UpdateShopAsync(int id, UpdateShopDto updateShopDto);
        Task<bool> DeleteShopAsync(int id);
    }
}
