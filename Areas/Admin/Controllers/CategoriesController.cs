using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using webbanhang.Areas.Admin.Attributes;
using webbanhang.Models;
using X.PagedList;
namespace webbanhang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class CategoriesController : Controller
    {
        public string strConnectionString;
        public CategoriesController()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            this.strConnectionString = config.GetConnectionString("MyConnectionString");
        }

        public IActionResult Index()
        {
            return RedirectToAction("Read");
        }
        public IActionResult Read(int? page)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from Categories where ParentId = 0 order by Id Desc", conn);
                da.Fill(dt);
            }
            List<ItemCategory> list = new List<ItemCategory>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(new ItemCategory() { Id = Convert.ToInt32(item["id"]), ParentId = Convert.ToInt32(item["ParentId"]), Name = item["Name"].ToString(), DisplayHomePage = Convert.ToInt32(item["DisplayHomePage"]) });
            }
            int page_size = 5;
            int page_num = page ?? 1;

            return View("Read", list.ToPagedList(page_num, page_size));
        }
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            ViewBag.action = "/Admin/Categories/UpdatePost/" + _id;
            DataTable dtCategory = new DataTable();
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from Categories where Id = " + _id, conn);
                da.Fill(dtCategory);
            }
            DataRow row = dtCategory.NewRow();
            if (dtCategory.Rows.Count > 0)
            {
                row = dtCategory.Rows[0];
            }
            return View("CreateUpdate", row);
        }
        [HttpPost]
        public IActionResult UpdatePost(int id, IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            int _ParentId = Convert.ToInt32(fc["ParentId"]);
            int _DisplayHomePage = !String.IsNullOrEmpty(fc["DisplayHomePage"]) ? 1 : 0;
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("update Categories set Name=@var_name, ParentId = @var_parent_id, DisplayHomePage = @var_display_home_page where Id = @var_id", conn);
                cmd.Parameters.AddWithValue("var_name", _Name);
                cmd.Parameters.AddWithValue("var_parent_id", _ParentId);
                cmd.Parameters.AddWithValue("var_display_home_page", _DisplayHomePage);
                cmd.Parameters.AddWithValue("var_id", id);
                //update
                cmd.ExecuteNonQuery();
            }
            return Redirect("/Admin/Categories");
        }
        public IActionResult Create()
        {
            ViewBag.action = "/Admin/Categories/CreatePost";
            return View("CreateUpdate");
        }
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            int _ParentId = Convert.ToInt32(fc["ParentId"]);
            int _DisplayHomePage = !String.IsNullOrEmpty(fc["DisplayHomePage"]) ? 1 : 0;
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into Categories(Name,ParentId,DisplayHomePage) values(@var_name,@var_parent_id,@var_display_home_page)", conn);
                cmd.Parameters.AddWithValue("var_name", _Name);
                cmd.Parameters.AddWithValue("var_parent_id", _ParentId);
                cmd.Parameters.AddWithValue("var_display_home_page", _DisplayHomePage);
                cmd.ExecuteNonQuery();
            }
            return Redirect("/Admin/Categories");
        }
        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from Categories where Id = @var_id", conn);
                cmd.Parameters.AddWithValue("var_id", id);

                cmd.ExecuteNonQuery();
            }
            return Redirect("/Admin/Categories");
        }
    }
}
