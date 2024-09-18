using Microsoft.AspNetCore.Mvc;
using webbanhang.Areas.Admin.Attributes;
using webbanhang.Models;
using X.PagedList;
namespace webbanhang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class AdminsController : Controller
    {
        public MyDBContext db = new MyDBContext();
        public IActionResult Index()
        {
            return RedirectToAction("Read");
        }
        public IActionResult Read(int? page)
        {
            int page_size = 8;
            int page_num = page ?? 1;
            var list_AdChecked = db.Admins.OrderByDescending(x => x.Id).ToList();
            return View("Read", list_AdChecked.ToPagedList(page_num, page_size));
        }
        public IActionResult Update(int? id)
        {

            int _id = id ?? 0;
            ViewBag.action = "/Admin/Admins/UpdatePost/" + _id;

            var AdChecked = db.Admins.FirstOrDefault(x => x.Id == id);
            return View("CreateUpdate", AdChecked);

        }
        [HttpPost]
        public IActionResult UpdatePost(int id, IFormCollection fc)
        {


            string _Name = fc["Name"].ToString().Trim();
            string _Password = fc["Password"].ToString().Trim();

            var AdChecked = (from Admin in db.Admins where Admin.Id == id select Admin).FirstOrDefault();
            if (AdChecked != null)
            {
                AdChecked.Name = _Name;
            }

            if (!String.IsNullOrEmpty(_Password))
            {
                _Password = BCrypt.Net.BCrypt.HashPassword(_Password);
                AdChecked.Password = _Password;
            }
            db.SaveChanges();
            return Redirect("/Admin/Admins");
        }
        public IActionResult Create()
        {

            ViewBag.action = "/Admin/Admins/CreatePost";
            return View("CreateUpdate");

        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {


            string _Name = fc["Name"].ToString().Trim();
            string _Email = fc["Email"].ToString().Trim();
            string _Password = fc["Password"].ToString().Trim();
            _Password = BCrypt.Net.BCrypt.HashPassword(_Password);
            var AdChecked = new ItemAdmin();
            AdChecked.Name = _Name;
            AdChecked.Email = _Email;
            AdChecked.Password = _Password;
            db.Admins.Add(AdChecked);
            db.SaveChanges();
            return Redirect("/Admin/Admins");
        }
        public IActionResult Delete(int id)
        {
            var AdChecked = db.Admins.FirstOrDefault(x => x.Id == id);
            db.Admins.Remove(AdChecked);
            db.SaveChanges();

            return Redirect("/Admin/Admins");
        }
    }
}
