using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Internet.Models
{
    public partial class Partner
    {
        public bool IsActive
        {
            get { return ExpirationDate > DateTime.Now; }
        }

        public MembershipUser User
        {
            get { return Membership.GetUser(UserId); }
        }

        public static Partner CurrentUserAsPartner
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated) return null;
                var user = Membership.GetUser(HttpContext.Current.User.Identity.Name);
                using (var db = new ffiEntities())
                {
                    var userId = new Guid(user.ProviderUserKey.ToString());
                    var partner = db.Partners.FirstOrDefault(entry => entry.UserId == userId);
                    return partner;
                }
            }
        }
    }
}