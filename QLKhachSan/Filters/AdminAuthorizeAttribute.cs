using System.Web.Mvc;
using System.Web.Routing;

namespace QLKhachSan.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            var session =
                filterContext.HttpContext.Session;

            var role =
                session["Role"] as string;

            // Chưa đăng nhập
            if (string.IsNullOrEmpty(role))
            {
                filterContext.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Account",
                                action = "Login"
                            }
                        )
                    );

                return;
            }

            // Đã đăng nhập nhưng không phải Admin
            if (role != "Admin")
            {
                filterContext.Result =
                    new HttpStatusCodeResult(403);
            }
        }
    }
}