using Microsoft.AspNetCore.Mvc;
using webbanhang.Models;
namespace webbanhang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        public MyDBContext db = new MyDBContext();
        public IActionResult Login()
        {
            ViewBag.action = "/Admin/Account/LoginPost";
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fc)
        {
            string _Email = fc["Email"].ToString().Trim();
            string _Password = fc["Password"].ToString().Trim();
            ItemAdmin itemAdmin = db.Admins.Where(x => x.Email == _Email).FirstOrDefault();
            if (itemAdmin != null)
            {
                if (BCrypt.Net.BCrypt.Verify(_Password, itemAdmin.Password))
                {
                    HttpContext.Session.SetString("admin_id", itemAdmin.Id.ToString());
                    HttpContext.Session.SetString("admin_email", itemAdmin.Email.ToString());
                    return RedirectToAction("Index", "Home");
                }

            }
            return Redirect("/Admin/Account/Login?notify=LoginFailed");

        }
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("admin_user_id");
            HttpContext.Session.Remove("admin_user_email");
            return Redirect("/Admin/Account/Login");
        }
    }
}
