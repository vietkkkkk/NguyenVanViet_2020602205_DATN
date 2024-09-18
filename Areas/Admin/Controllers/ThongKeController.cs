using Microsoft.AspNetCore.Mvc;
using webbanhang.Models; //nhin thay cac file .cs trong folder Models
using System.Data;//su dung DataTable
using X.PagedList; //phan trang
using webbanhang.Areas.Admin.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax; //nhin thay cac file .cs trong folder Attributes

namespace webbanhang.Areas.Admin.Controllers
{
        [Area("Admin")]
        //Kiem tra login
        [CheckLogin]
    public class ThongKeController : Controller
    {
        public MyDBContext db = new MyDBContext();
        public IActionResult Index(int? page)
        {
            //lay trang  hien tai
            /*
             page ?? 1
                neu page khac null thi _CurrentPage = page
                neu page = null thi _CurrentPage = 1
             */
            int _CurrentPage = page ?? 1;
            //dinh nghia so ban ghi tren mot trang
            int _RecordPerPage = 20;
            //lay tat ca cac ban ghi trong table News
            List<ItemOrder> listRecord = db.Orders.OrderByDescending(item => item.Id).ToList();
            //truyen gia tri ra view co phan trang
            //return Content(HttpContext.Session.GetString("id"));
            return View("Index", listRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        //chi tiet san pham
    }
}
