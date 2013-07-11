using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Internet.Extensions;
using Internet.Helpers;
using Internet.Models;

namespace Internet.Controllers
{
    public class HomeController : Controller
    {
        private ffiEntities _db = new ffiEntities();

        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            System.Web.HttpContext.Current.Session["Culture"] = lang;
            return Redirect(returnUrl);
        }

//        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult Index(string nickname)
        {
          /*   string queryString = 
        "Select @@version";
            using (SqlConnection connection = new SqlConnection(
                ((EntityConnection)_db.Connection).StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                connection.Open();
                var version = command.ExecuteScalar();
            }*/

//            return RedirectToAction("Maintance");
            var slides = _db.Slides.ToList();
            ViewBag.Slides = slides;
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }

        public ActionResult Maintance()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contacts(string toAddress,string name, string eMail, string text)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(eMail) && !string.IsNullOrEmpty(text))
            {
                EmailHelper.Instance.SendContactsEmail(toAddress, name, eMail, text);
                return View().Warning(@Resources.labels.ContactEmailSent);
            }
            return View().Warning(@Resources.labels.SendMessageFail);
        }
    }
}
