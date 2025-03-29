using POS.Shared.DTOs;
using POS.Shared;

namespace POS.Web.MVC.Services.IService
{
    public interface IShopService
    {
        Task<PagedResultDto<ShopDto>> GetShopsAsync(ShopSearchDto searchDto);
    }
}
