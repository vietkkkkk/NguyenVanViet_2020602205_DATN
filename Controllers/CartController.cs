using Microsoft.AspNetCore.Mvc;
using webbanhang.Models;
using X.PagedList;
using Newtonsoft.Json;

namespace project.Controllers
{
    public class CartController : Controller
    {
        public MyDBContext db = new MyDBContext();
        public IActionResult Index()
        {
            return RedirectToAction("Read");
        }
        public IActionResult Read()
        {
           
            string json_cart = HttpContext.Session.GetString("cart");
        
            List<Item> cart = new List<Item>();
            if (!String.IsNullOrEmpty(json_cart))
            {
                cart = JsonConvert.DeserializeObject<List<Item>>(json_cart);
            }
          
            return View("Read", cart);
        }
        //thêm sản phẩm vào giỏ hàng
        public IActionResult Add(int id)
        {
            //gọi hàm CartAdd để thêm sản phẩm vào giỏ hàng
            Cart.CartAdd(HttpContext.Session, id);
            return Redirect("/Cart/Read");
        }
        //xóa sản phẩm khỏi giỏ hàng
        public IActionResult Delete(int id)
        {
            Cart.CartRemove(HttpContext.Session, id);
            return Redirect("/Cart/Read");
        }
        //update số lượng sản phẩm trong giỏ hàng
        public IActionResult Update()
        {
            //lấy chuỗi json có tên là cart tại biến session
            string json_cart = HttpContext.Session.GetString("cart");
            //tạo List cart để chuẩn bị đổ dữ liệu từ session vào
            List<Item> cart = new List<Item>();
            if (!String.IsNullOrEmpty(json_cart))
            {
                cart = JsonConvert.DeserializeObject<List<Item>>(json_cart);
                //duyệt các phần tử trong giỏ hàng
                foreach (var item in cart)
                {
                    int quantity = Convert.ToInt32(Request.Form["product_" + item.ProductRecord.Id]);
                    //gọi hàm update
                    Cart.CartUpdate(HttpContext.Session, item.ProductRecord.Id, quantity);
                }
            }
            return Redirect("/Cart/Read");
        }
        //xóa toàn bộ sản phẩm trong giỏ hàng
        public IActionResult Destroy()
        {
            Cart.CartDestroy(HttpContext.Session);
            return Redirect("/Cart/Read");
        }
        //thanh toán đơn hàng
        public IActionResult Checkout()
        {
            //kiểm tra xem user đã đăng nhập thì mới mua được hàng
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("customer_user_id")))
                return Redirect("/Account/Login");
            else
                Cart.CartCheckOut(HttpContext.Session, Convert.ToInt32(HttpContext.Session.GetString("customer_user_id")));
            return Redirect("/Cart/Read");
        }
    }
}
