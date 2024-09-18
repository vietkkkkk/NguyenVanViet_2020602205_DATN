using Microsoft.AspNetCore.Mvc;

namespace webbanhang.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
