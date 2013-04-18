using System.Linq;
using System.Web.Mvc;
using Internet.Models;

namespace Internet.Controllers
{
    public class HomeController : Controller
    {
        private ffiEntities _db = new ffiEntities();

        public ActionResult Index()
        {
            var categories = _db.Categories.OrderBy(entry => entry.Index).ToList();
            return View(categories);
        }

        public ActionResult About()
        {
            return View();
        }  
        
        public ActionResult TestDrive()
        {
            return View();
        }

        public ActionResult Instruction()
        {
            return View();
        }
        
        public ActionResult Contacts()
        {
            return View();
        }
    }
}
