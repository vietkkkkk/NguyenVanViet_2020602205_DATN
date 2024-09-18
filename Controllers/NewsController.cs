using Microsoft.AspNetCore.Mvc;
using webbanhang.Models;
using X.PagedList;
using X.PagedList.Mvc;
namespace webbanhang.Controllers
{
    public class NewsController : Controller
    {
        public MyDBContext db = new MyDBContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult News(int? page)
        {
            List<ItemNews> record = db.News.OrderByDescending(x => x.Id).ToList();
            int page_size = 9;
            int page_number = page ?? 1;
            return View("News",record.ToPagedList(page_number,page_size));
        }
        public IActionResult Detail(int? id) {

            int _id = id ?? 0;
            ItemNews record = db.News.Where(item => item.Id == _id).FirstOrDefault();
            return View("Detail", record);

        }
    }
}
