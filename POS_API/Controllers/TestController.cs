using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS_API.Services.IServices;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public TestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("send")]
        public async Task<IActionResult> SendEmail()
        {
            await _emailService.QueueEmailAsync(
                "nguyenkhaclong12a8pkk@gmail.com",
                "Thông báo mới",
                "<p>Đây là nội dung <strong>HTML</strong> của email</p>",
                true);

            return Ok("Email sent successfully.");
        }
    }
}
