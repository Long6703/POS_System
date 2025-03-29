using POS.Shared;
using POS.Shared.DTOs;
using POS.Web.MVC.Services.IService;
using System.Net.Http;

namespace POS.Web.MVC.Services
{
    public class ShopService : IShopService
    {
        private readonly BaseApiService _baseApiService;

        public ShopService(BaseApiService baseApiService)
        {
            _baseApiService = baseApiService;
        }

        public async Task<PagedResultDto<ShopDto>> GetShopsAsync(ShopSearchDto searchDto)
        {
            var response = await _baseApiService.GetAsync<PagedResultDto<ShopDto>>(
                $"shop?Name={searchDto.Name}&Address={searchDto.Address}&PageNumber={searchDto.PageNumber}&PageSize={searchDto.PageSize}&SortBy={searchDto.SortBy}&IsDescending={searchDto.IsDescending}");

            return response ?? new PagedResultDto<ShopDto>(new List<ShopDto>(), 0, searchDto.PageNumber, searchDto.PageSize);
        }
    }
}
