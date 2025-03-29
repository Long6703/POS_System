using Microsoft.AspNetCore.Mvc;

namespace POS.Web.MVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
