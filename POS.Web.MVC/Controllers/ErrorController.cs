using Microsoft.AspNetCore.Mvc;

namespace POS.Web.MVC.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/Forbidden")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [Route("Error/SomethingWentWrong")]
        public IActionResult SomethingWentWrong()
        {
            return View();
        }
    }
}
