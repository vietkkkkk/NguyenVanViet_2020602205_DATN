using Microsoft.AspNetCore.Mvc.Filters;
namespace webbanhang.Areas.Admin.Attributes
{
    public class CheckLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            base.OnActionExecuting(context);
            if (String.IsNullOrEmpty(context.HttpContext.Session.GetString("admin_email")))
            {
                context.HttpContext.Response.Redirect("/Admin/Account/Login");
                base.OnActionExecuting(context);
            }
        }
    }
}
