using System.Web.Mvc;
using Internet.RoutingHelpers;

namespace Internet.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional },
                new { adminConstraint = new AreaRouteConstraint { AreaName = context.AreaName } },
                new[] { "Internet.Areas.Admin.Controllers" }
            );
        }
    }
}
