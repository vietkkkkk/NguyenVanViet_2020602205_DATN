using Microsoft.AspNetCore.Mvc;
//đối tượng phân trang
using X.PagedList;
//Để nhìn thấy các file trong thư mục Models
using webbanhang.Models;

using webbanhang.Areas.Admin.Attributes;
using webbanhang.Models;

namespace webbanhang.Areas.Admin.Controllers
{
    //trong area admin phải có dòng dưới
    [Area("Admin")]
    [CheckLogin]
    public class ProductsController : Controller
    {
        //khởi tạo đối tượng để thao tác csdl
        public MyDBContext db = new MyDBContext();
        public IActionResult Index()
        {
            //di chuyển đến hàm Read()
            return RedirectToAction("Read", "Products");
        }
        public IActionResult Read(int? page)
        {
            //thiết lập số bản ghi trên một trang
            int page_size = 4;
            //thiết lập trang hiện tại
            int page_number = page ?? 1;//nếu p là null thì page_number=1, không thì page_number=p
            //sử dụng linq để lấy các bản ghi
            var list_record = db.Products.OrderByDescending(item => item.Id).ToList();
            //var list_record = (from item in db.Users orderby item.Id descending select item).ToList();
            return View("Read", list_record.ToPagedList(page_number, page_size));
        }
        //update
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //khai báo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Products/UpdatePost/" + _id;
            //lấy một bản ghi
            //var record = db.Users.FirstOrDefault(item => item.Id == id);
            var record = db.Products.Where(item => item.Id == id).FirstOrDefault();
            return View("CreateUpdate", record);
        }
        //update Post
        [HttpPost]
        public IActionResult UpdatePost(int id, IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Description = fc["Description"].ToString().Trim();
            string _Content = fc["Content"].ToString().Trim();
            double _Price = Convert.ToDouble(fc["Price"].ToString());
            int _Hot = !String.IsNullOrEmpty(fc["Hot"]) ? 1 : 0;
            double _Discount = Convert.ToDouble(fc["Discount"].ToString());
            //---
            var record = db.Products.Where(item => item.Id == id).FirstOrDefault();
            if (record != null)
            {
                record.Name = _Name;
                record.Description = _Description;
                record.Content = _Content;
                record.Price = _Price;
                record.Hot = _Hot;
                record.Discount = _Discount;
                //update
                db.SaveChanges();
                try
                {
                    //kiểm tra ảnh để thực hiện upload ảnh
                    if (!String.IsNullOrEmpty(Request.Form.Files[0].FileName))
                    {
                        //return Content("ok");
                        //lấy tên file
                        string _Photo = Request.Form.Files[0].FileName;
                        //thêm chuỗi để tạo ra tên file khác nhau
                        _Photo = DateTime.Now.ToFileTime() + "_" + _Photo;
                        //nối chuỗi "wwwroot/Upload/Products/" và chuỗi ở biến _Photo để thành một đường dẫn hoàn chỉnh
                        string _Path = Path.Combine("wwwroot/Upload/Products/", _Photo);
                        //upload file
                        using (var stream = new FileStream(_Path, FileMode.Create))
                        {
                            Request.Form.Files[0].CopyTo(stream);
                        }
                        //cập nhật lại trường Photo của talbe Products
                        record.Photo = _Photo;
                        db.SaveChanges();
                    }
                }
                catch {; }
                //gọi hàm để update Categories
                CreateUpdateCategoriesProducts(id);
                //gọi hàm để update Tags
                CreateUpdateTagsProducts(id);
            }
            //---
            //di chuyển đến một url
            return Redirect("/Admin/Products");
        }
        public void CreateUpdateCategoriesProducts(int _ProductId)
        {
            //lấy giá trị của biến form có name=Categories
            List<string> categories = Request.Form["Categories"].ToList();
            //xóa hết các bản ghi tương ứng với _ProductId
            List<ItemCategoryProduct> list_categories_products = db.CategoriesProducts.Where(item => item.ProductId == _ProductId).ToList();
            foreach (var item in list_categories_products)
            {
                db.CategoriesProducts.Remove(item);
                db.SaveChanges();
            }
            //---
            foreach (string category in categories)
            {
                int _CategoryId = Convert.ToInt32(category);
                //thêm mới bản ghi vào table CategoriesProducts
                ItemCategoryProduct record = new ItemCategoryProduct();
                record.ProductId = _ProductId;
                record.CategoryId = _CategoryId;
                db.CategoriesProducts.Add(record);
                db.SaveChanges();
            }
        }
        public void CreateUpdateTagsProducts(int _ProductId)
        {
            //lấy giá trị của biến form có name=Categories
            List<string> tags = Request.Form["Tags"].ToList();
            //xóa hết các bản ghi tương ứng với _ProductId
            List<ItemTagProduct> list_tags_products = db.TagsProducts.Where(item => item.ProductId == _ProductId).ToList();
            foreach (var item in list_tags_products)
            {
                db.TagsProducts.Remove(item);
                db.SaveChanges();
            }
            //---
            foreach (string tag in tags)
            {
                int _TagId = Convert.ToInt32(tag);
                //thêm mới bản ghi vào table CategoriesProducts
                ItemTagProduct record = new ItemTagProduct();
                record.ProductId = _ProductId;
                record.TagId = _TagId;
                db.TagsProducts.Add(record);
                db.SaveChanges();
            }
        }
        //create
        public IActionResult Create()
        {
            //khai báo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Products/CreatePost";
            return View("CreateUpdate");
        }
        //create Post
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Description = fc["Description"].ToString().Trim();
            string _Content = fc["Content"].ToString().Trim();
            double _Price = Convert.ToDouble(fc["Price"].ToString());
            int _Hot = !String.IsNullOrEmpty(fc["Hot"]) ? 1 : 0;
            double _Discount = Convert.ToDouble(fc["Discount"].ToString());
            string _Photo = "";
            //---
            //kiểm tra ảnh để thực hiện upload ảnh
            try
            {
                if (!String.IsNullOrEmpty(Request.Form.Files[0].FileName))
                {
                    //return Content("ok");
                    //lấy tên file
                    _Photo = Request.Form.Files[0].FileName;
                    //thêm chuỗi để tạo ra tên file khác nhau
                    _Photo = DateTime.Now.ToFileTime() + "_" + _Photo;
                    //nối chuỗi "wwwroot/Upload/Products/" và chuỗi ở biến _Photo để thành một đường dẫn hoàn chỉnh
                    string _Path = Path.Combine("wwwroot/Upload/Products/", _Photo);
                    //upload file
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                }
            }
            catch {; }
            //---
            var record = new ItemProduct();
            if (record != null)
            {
                record.Name = _Name;
                record.Description = _Description;
                record.Content = _Content;
                record.Price = _Price;
                record.Hot = _Hot;
                record.Photo = _Photo;
                record.Discount = _Discount;
                //insert
                db.Products.Add(record);
                db.SaveChanges();
                //lấy id vừa mới insert
                int _ProductId = record.Id;
                //gọi hàm để Create Categories
                CreateUpdateCategoriesProducts(_ProductId);
                //gọi hàm để Create Tags
                CreateUpdateTagsProducts(_ProductId);
            }
            //---
            //di chuyển đến một url
            return Redirect("/Admin/Products");
        }
        //xóa bản ghi
        public IActionResult Delete(int id)
        {
            //lấy một bản ghi để update
            var record = db.Products.Where(item => item.Id == id).FirstOrDefault();
            //xóa bản ghi
            db.Products.Remove(record);
            db.SaveChanges();
            //di chuyển đến một url
            return Redirect("/Admin/Products");
        }
    }
}
