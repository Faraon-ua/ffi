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

        public ActionResult Index()
        {
            var slides = _db.Slides.ToList();
            ViewBag.Slides = slides;
            var categories = _db.Categories.OrderBy(entry => entry.Index).ToList();
            return View(categories);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contacts(string name, string eMail, string text)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(eMail) && !string.IsNullOrEmpty(text))
            {
                EmailHelper.Instance.SendContactsEmail(name, eMail, text);
                return View().Warning(@Resources.labels.ContactEmailSent);
            }
            return View().Warning(@Resources.labels.SendMessageFail);
        }
    }
}
