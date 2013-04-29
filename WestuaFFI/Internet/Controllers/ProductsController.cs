using System;
using System.Linq;
using System.Web.Mvc;
using Internet.Models;

namespace Internet.Controllers
{ 
    public class ProductsController : Controller
    {
        private ffiEntities db = new ffiEntities();

        public ActionResult GetImage(Guid productId)
        {
            var product = db.Products.FirstOrDefault(entry => entry.Id == productId);
            var arr = product.Image == null ? new byte[0] : product.Image;
            return File(arr, "image/png");
        }
        //
        // GET: /Products/

        public PartialViewResult Index(Guid categoryId)
        {
            var products = db.Products.Include("Category").Where(entry=>entry.CategoryId == categoryId);
            return PartialView("_Products", products.ToList());
        }

        //
        // GET: /Products/Details/5

        public ViewResult Details(Guid id)
        {
            Product product = db.Products.Single(p => p.Id == id);
            return View(product);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}