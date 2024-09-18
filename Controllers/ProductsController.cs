using Microsoft.AspNetCore.Mvc;
using webbanhang.Models;
using X.PagedList;

namespace webbanhang.Controllers
{
    public class ProductsController : Controller
    {
        public MyDBContext db = new MyDBContext();
        public IActionResult Categories(int? id, int? page)
        {
            int _CategoryId = id ?? 0;
            //return Content(_CategoryId.ToString());
            //truyền biến CategoryId ra bên ngoài view
            ViewBag._CategoryId = _CategoryId;
            //lấy các sản phẩm trong trường hợp không có CategoryId
            List<ItemProduct> list_record = db.Products.OrderByDescending(item => item.Id).ToList();
            //nếu có CategoryId thì thêm một bước truy vấn linq để lọc theo CategoryId
            if (_CategoryId > 0)
            {
                list_record = (from product in db.Products join category_product in db.CategoriesProducts on product.Id equals category_product.ProductId join category in db.Categories on category_product.CategoryId equals category.Id where category.Id == _CategoryId select product).OrderByDescending(item => item.Id).ToList();
            }
            string orderby = !String.IsNullOrEmpty("orderby") ? Request.Query["orderby"] : "";
            ViewBag.orderby = orderby;
            switch (orderby)
            {
                case "name-asc":
                    list_record = list_record.OrderBy(item => item.Name).ToList();
                    break;
                case "name-desc":
                    list_record = list_record.OrderByDescending(item => item.Name).ToList();
                    break;
                case "price-asc":
                    list_record = list_record.OrderBy(item => item.Price).ToList();
                    break;
                case "price-desc":
                    list_record = list_record.OrderByDescending(item => item.Price).ToList();
                    break;
            }
            //phan trang
            int page_size = 9;
            int page_number = page ?? 1;
            return View("Categories", list_record.ToPagedList(page_number, page_size));
        }
        public IActionResult Detail(int? id)
        {
            int _id = id ?? 0;
            ItemProduct record = db.Products.Where(item => item.Id == _id).FirstOrDefault();
            return View("Detail", record);
        }
        public IActionResult Rate(int? id)
        {
            int _id = id ?? 0;
            int _star = !String.IsNullOrEmpty(Request.Query["star"]) ? Convert.ToInt32(Request.Query["star"]) : 0;
            ItemRating record = new ItemRating();
            record.ProductId = _id;
            record.Star = _star;
            db.Add(record);
            db.SaveChanges();
            return Redirect("/Products/Detail/" + _id);
        }
        public IActionResult Search(int? page)
        {

            double fromPrice = !String.IsNullOrEmpty(Request.Query["fromPrice"]) ? Convert.ToDouble(Request.Query["fromPrice"]) : 0;
            double toPrice = !String.IsNullOrEmpty(Request.Query["toPrice"]) ? Convert.ToDouble(Request.Query["toPrice"]) : 0;
            int pageSize = 9;
            int pageNumber = page ?? 1;
            List<ItemProduct> list_record = db.Products.Where(x => x.Price >= fromPrice && x.Price <= toPrice).ToList();

            ViewBag.fromPrice = fromPrice;
            ViewBag.toPrice = toPrice;

            return View("Search", list_record.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Tag(int? page)
        {
            int tag_id = !String.IsNullOrEmpty(Request.Query["tag_id"]) ? Convert.ToInt32(Request.Query["tag_id"]) : 0;
            int pageSize = 9;
            int pageNumber = page ?? 1;
            List<ItemProduct> list_record = db.Products.OrderByDescending(item => item.Id).ToList();

            list_record = (from product in db.Products join tag_product in db.TagsProducts on product.Id equals tag_product.ProductId join tags in db.Categories on tag_product.TagId equals tags.Id where tags.Id == tag_id select product).OrderByDescending(item => item.Id).ToList();
            ViewBag.tag_id = tag_id;

            return View("Tag", list_record.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult SearchName(int? page)
        {

            string key = !String.IsNullOrEmpty(Request.Query["key"]) ? Request.Query["key"] : "";

            int pageSize = 9;
            int pageNumber = page ?? 1;
            List<ItemProduct> list_record = db.Products.Where(x => x.Name.Contains(key)).ToList();

            ViewBag.key = key;


            return View("SearchName", list_record.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult SearchAjax()
        {
            string key = !String.IsNullOrEmpty(Request.Query["key"]) ? Request.Query["key"] : "";
            List<ItemProduct> list_record = db.Products.Where(item => item.Name.Contains(key)).ToList();
            string str = "";
            foreach (var item in list_record)
            {
                str += "<li class='search-item'><img style='margin-right: 10px;' src=\"/Upload/Products/" + item.Photo + "\" /><a href=\"/Products/Detail/" + item.Id + "\">" + item.Name + "</a></li>";
            }
            return Content(str);

        }
    }
}
