using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Internet.RoutingHelpers;

namespace Internet
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Internet.Controllers" }
            );

            //TODO: remove if not used
            //            routes.Add("DomainRoute", new DomainRoute(
            //                "{nickname}.localhost/Internet/",     // Domain with parameters
            //                "{controller}/{action}/{id}",    // URL with parameters
            //                new { nickname = "en", controller = "Home", action = "Index", id = "" }  // Parameter defaults
            //            ));
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        void Session_Start(object sender, EventArgs e)
        {
            var partner = SubdomainHelper.GetSiteOwner();
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["Culture"] != null)
            {
                culture = Convert.ToString(HttpContext.Current.Session["Culture"]);
            }
            var cultureInfo = new System.Globalization.CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }


    }
}