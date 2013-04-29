using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using Internet.Extensions;
using Internet.Helpers;

namespace Internet.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Admin/Users/

        public ActionResult Index()
        {
            var usersToVerify =
                Membership.GetAllUsers().OfType<MembershipUser>().Where(entry => entry.IsApproved == false);
            return View(usersToVerify);
        }

        public ActionResult ApproveUser(string userName)
        {
            var user = Membership.GetUser(userName);
            if (user != null)
            {
                EmailHelper.Instance.SendConfirmationEmail(userName);
            }
            return RedirectToAction("Index").Warning("Email with confirmation sent to " + userName);
        }

        public ActionResult RemoveUser(string userName)
        {
            var user = Membership.GetUser(userName);
            if (user != null)
            {
                Membership.DeleteUser(userName);
            }
            return RedirectToAction("Index").Warning(userName + "was removed");
        }
    }
}
