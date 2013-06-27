using System.Web;
using System.Web.Routing;

namespace Internet.RoutingHelpers
{
    public class AreaRouteConstraint : IRouteConstraint
    {
        public const string AdminAreaName = "Admin";
        public const string AdminRoleName = "Administrator";

        public string AreaName { get; set; }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (AreaName != null)
                if (AreaName == AdminAreaName)
                    if (httpContext.User.Identity.IsAuthenticated)
                        if (httpContext.User.IsInRole(AdminRoleName))
                            return true;
            return false;
        }
    }
}