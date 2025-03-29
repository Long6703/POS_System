using Microsoft.AspNetCore.Components;
using POS.Shared;
using POS.Shared.DTOs;
using POS.Web.Services;
using System.Threading.Tasks;

namespace POS.Web.Pages.Admin
{
    public partial class ShopList
    {
        private ShopSearchDto _searchDto = new() { PageNumber = 1, PageSize = 5 };
        private PagedResultDto<ShopDto> _shopList = new(new List<ShopDto>(), 0, 1, 5);

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (!user.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo("/login");
            }
            await LoadShops();
        }

        private async Task LoadShops()
        {
            var result = await ApiService.GetAsync<PagedResultDto<ShopDto>>($"api/shops?Name={_searchDto.Name}&Address={_searchDto.Address}&PageNumber={_searchDto.PageNumber}&PageSize={_searchDto.PageSize}");
            if (result != null)
            {
                _shopList = result;
            }
        }

        private async Task SearchShops()
        {
            _searchDto.PageNumber = 1;
            await LoadShops();
        }

        private async Task NextPage()
        {
            if (_shopList.HasNext)
            {
                _searchDto.PageNumber++;
                await LoadShops();
            }
        }

        private async Task PreviousPage()
        {
            if (_shopList.HasPrevious)
            {
                _searchDto.PageNumber--;
                await LoadShops();
            }
        }
    }
}
