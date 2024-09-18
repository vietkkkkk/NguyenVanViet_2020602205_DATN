using Microsoft.AspNetCore.Mvc;
namespace webbanhang.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
            //  return Content(BCrypt.Net.BCrypt.HashPassword("admin123"));
        }
    }
}
