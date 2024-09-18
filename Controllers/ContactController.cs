using Microsoft.AspNetCore.Mvc;

namespace webbanhang.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
