using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Shared.RequestModel;
using POS_API.Services.IServices;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.Login(request);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Login failed.");
            }
            return Ok(new {token = result});
        }
    }
}
