using Microsoft.AspNetCore.Mvc;

namespace POS.Web.MVC.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
