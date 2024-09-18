using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using X.PagedList; // doi tuong phan trang
using webbanhang.Models;
using webbanhang.Areas.Admin.Attributes;


namespace webbanhang.Controllers
{
    public class AccountController : Controller
    {
        public MyDBContext db = new MyDBContext();
        public IActionResult Register()
        {
            ViewBag.action = "/Account/RegisterPost";
            return View();
        }
        [HttpPost]
        public IActionResult RegisterPost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString();
            string _Email = fc["Email"].ToString();
            string _Phone = fc["Phone"].ToString();
            string _Address = fc["Address"].ToString();
            string _Password = fc["Password"].ToString();
            var check = db.Customers.Where(item => item.Email == _Email).FirstOrDefault();
            if (check == null)
            {
                var record = new ItemCustomer();
                record.Email = _Email;
                record.Phone = _Phone;
                record.Address = _Address;
                record.Password = BCrypt.Net.BCrypt.HashPassword(_Password);
                record.Name = _Name;
                db.Customers.Add(record);
                db.SaveChanges();
                return Redirect("/Account/Login?notify=register-success");
            }
            else
                return Redirect("/Account/Register?notify=email-exists");


        }
        public IActionResult Login()
        {
            ViewBag.action = "/Account/LoginPost";
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fc)
        {
            string _Email = fc["Email"].ToString();
            string _Password = fc["Password"].ToString();
            var record = db.Customers.Where(item => item.Email == _Email).FirstOrDefault();
            if (record != null)
            {
                if (BCrypt.Net.BCrypt.Verify(_Password, record.Password))
                {
                    HttpContext.Session.SetString("customer_user_id", record.Id.ToString());
                    HttpContext.Session.SetString("customer_user_email", record.Email.ToString());
                    HttpContext.Session.SetString("customer_user_name", record.Name.ToString());
                    return Redirect("/Home/Index");
                }
                else return Redirect("/Account/Login?notify=invalid_email");
            }
            else return Redirect("/Account/Login?notify=invalid_email");

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customer_user_id");
            HttpContext.Session.Remove("customer_user_email");
            return Redirect("/Account/Login");
        }
        public IActionResult EditProfile()
        {
            ViewBag.action = "/Account/EditProfilePost";
            return View();
        }
        [HttpPost]
        public IActionResult EditProfilePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString();
            string _Phone = fc["Phone"].ToString();
            string _Address = fc["Address"].ToString();
            string _Password = fc["Password"].ToString();
          
            var record = db.Customers.Where(item => item.Email == HttpContext.Session.GetString("customer_user_email")).FirstOrDefault();
            record.Name = _Name;
            record.Phone = _Phone;
            record.Address = _Address;
            //nếu Password không rỗng thì update password
            if (!String.IsNullOrEmpty(_Password))
                record.Password = BCrypt.Net.BCrypt.HashPassword(_Password);
            db.SaveChanges();
            //sau khi đăng ký thành công sẽ di chuyển đi trang login
            return Redirect("/Account/Login");
        }
        public IActionResult Orders()
        {
            
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("customer_user_id")))
                return Redirect("/Account/Login");

            List<ItemOrder> list_record = db.Orders.OrderByDescending(item => item.Id).ToList();
              
            return View("Orders", list_record);
        }

        public IActionResult OrderDetail(int? id)
        {
            int _OrderId = id ?? 0;
            ViewBag.OrderId = _OrderId;
       
            List<ItemOrderDetail> _ListRecord = db.OrdersDetail.Where(tbl => tbl.OrderId == _OrderId).ToList();
            return View("OrderDetail", _ListRecord);
        }
     
        public IActionResult CancelOrder(int id)
        {
            
            ItemOrder record = db.Orders.FirstOrDefault(item => item.Id == id);
            record.Status = 2;
            db.SaveChanges();
            return Redirect("/Account/Orders");
        }
    }
}
