using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Internet.Helpers;
using Internet.Models;

namespace Internet.RoutingHelpers
{
    public class SubdomainHelper
    {
        public static Partner GetSiteOwner()
        {
            var db = new ffiEntities();
            var partner = new Partner();
            try
            {
                var host = HttpContext.Current.Request.Url.Host.Replace("www.", "");
                var subDomain = host.Substring(0, host.IndexOf(AppSettings.DomainName));

                if (subDomain.Length > 0 && subDomain != "www.")
                {
                    var user = Membership.GetUser(subDomain.Replace(".", ""));
                    var userGuid = new Guid(user.ProviderUserKey.ToString());
                    partner = db.Partners.FirstOrDefault(entry => entry.UserId == userGuid);
                    if (partner != null)
                        if (partner.IsActive) return partner;
                    HttpContext.Current.Response.Redirect("http://" + AppSettings.DomainName);
                }
                else
                {
                    var user = Membership.GetUser(Roles.GetUsersInRole(AreaRouteConstraint.AdminRoleName).FirstOrDefault(entry => entry.ToLower() == "mr1970"));
                    var userGuid = new Guid(user.ProviderUserKey.ToString());
                    partner = db.Partners.FirstOrDefault(entry => entry.UserId == userGuid);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return partner;
        }
    }
}