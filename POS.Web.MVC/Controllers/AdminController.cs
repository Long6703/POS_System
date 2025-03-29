using Microsoft.AspNetCore.Mvc;
using POS.Shared.DTOs;
using POS.Web.MVC.Services.IService;

namespace POS.Web.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IShopService _shopService;

        public AdminController(IShopService shopService)
        {
            _shopService = shopService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ShopList(string? name, string? address, int pageNumber = 1, int pageSize = 10, string sortBy = "Id", bool isDescending = false)
        {
            var searchDto = new ShopSearchDto
            {
                Name = name ?? string.Empty,
                Address = address ?? string.Empty,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortBy = sortBy,
                IsDescending = isDescending
            };

            var shops = await _shopService.GetShopsAsync(searchDto);
            return View(shops);
        }
    }
}
