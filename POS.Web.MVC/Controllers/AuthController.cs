using Microsoft.AspNetCore.Mvc;
using POS.Shared.RequestModel;
using POS.Web.MVC.Services.IService;

namespace POS.Web.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ISessionService _sessionService;

        public AuthController(IAuthService authService, ISessionService sessionService)
        {
            _authService = authService;
            _sessionService = sessionService;
        }


        [Route("Auth/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Auth/Login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = await _authService.LoginAsync(model);
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            await _sessionService.SetStringAsync("AuthToken", token);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Auth/Logout")]
        public IActionResult Logout()
        {
            _sessionService.SetStringAsync("AuthToken", string.Empty);
            return RedirectToAction("Login");
        }
    }
}
