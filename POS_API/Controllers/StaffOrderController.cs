using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Shared.DTOs.Order;
using POS_API.Services.IServices;

namespace POS_API.Controllers
{
    [Route("staff/orders")]
    [ApiController]
    [Authorize(Roles = "ShopOwner,Manager,Staff")]
    public class StaffOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public StaffOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
        {
            var result = await _orderService.CreateOrderAsync(createOrderDTO);
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderDTO updateOrderDTO)
        {
            var result = await _orderService.UpdateOrderAsync(updateOrderDTO);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> ConfirmPayment(Guid id, [FromBody] CreatePaymentDTO createPaymentDTO)
        {
            createPaymentDTO.OrderId = id;
            var result = await _orderService.ConfirmPaymentAsync(createPaymentDTO);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffOrders([FromQuery] OrderSearchDTO searchDTO)
        {
            var result = await _orderService.GetStaffOrdersAsync(searchDTO);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
