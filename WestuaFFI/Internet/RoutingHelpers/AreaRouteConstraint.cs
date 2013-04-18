using System.Web;
using System.Web.Routing;

namespace Internet.RoutingHelpers
{
    public class AreaRouteConstraint : IRouteConstraint
    {

        private const string AdminAreaName = "Admin";
        private const string AdminName = "westua";

        public string AreaName { get; set; }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (AreaName != null)
                if (AreaName == AdminAreaName)
                    if (httpContext.User.Identity.IsAuthenticated)
                        if (httpContext.User.Identity.Name == AdminName)
                            return true;
            return false;
        }
    }
}