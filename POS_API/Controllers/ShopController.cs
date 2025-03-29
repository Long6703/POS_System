using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Shared.DTOs;
using POS.Shared;
using POS_API.Services.IServices;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResultDto<ShopDto>>> GetShops([FromQuery] ShopSearchDto searchDto)
        {
            var shops = await _shopService.GetShopsAsync(searchDto);
            return Ok(shops);
        }


        [HttpPost]
        public async Task<ActionResult<ShopDto>> CreateShop([FromBody] CreateShopDto createShopDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdShop = await _shopService.CreateShopAsync(createShopDto);
            if (!createdShop)
            {
                return BadRequest("Error");
            }
            return Ok("Create Done");
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ShopDto>> GetShop(int id)
        {
            var shop = await _shopService.GetShopByIdAsync(id);
            if (shop == null)
            {
                return NotFound();
            }

            return Ok(shop);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(int id, [FromBody] UpdateShopDto updateShopDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _shopService.UpdateShopAsync(id, updateShopDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var result = await _shopService.DeleteShopAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
