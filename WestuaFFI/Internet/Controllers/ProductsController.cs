using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Internet.Extensions;
using Internet.Helpers;
using Internet.Models;
using Internet.RoutingHelpers;

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

        public ActionResult Index()
        {
            var categories = db.Categories.OrderBy(entry => entry.Index).ToList();
            return View(categories);
        }

        public PartialViewResult CategoryProducts(Guid categoryId)
        {
            Session["SelectedCategory"] = categoryId.ToString();
            var products = db.Products.Include("Category").Where(entry => entry.CategoryId == categoryId);
            return PartialView("_Products", products.OrderBy(entry=>entry.Name_en).ToList());
        }

        //
        // GET: /Products/Details/5

        public ViewResult Details(string id)
        {
            Product product = db.Products.Single(p => p.Name_en == id);
            return View(product);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult AddToCart(Guid id)
        {
            Dictionary<Product, int> cart;
            if (Session["Cart"] != null)
                cart = Session["Cart"] as Dictionary<Product, int>;
            else
                cart = new Dictionary<Product, int>();
            var product = db.Products.FirstOrDefault(entry => entry.Id == id);
            if (product != null)
            {
                if (cart.Keys.Contains(product, new ProductEqualityComparer()))
                {
                    var cartEntry = cart.FirstOrDefault(entry => entry.Key.Id == product.Id);
                    cart[cartEntry.Key] += 1;
                }
                else
                    cart.Add(product, 1);
            }
            Session["Cart"] = cart;
            return RedirectToAction("Cart", "Products");
        }

        public ActionResult CartRefresh(IDictionary<Guid, int> cartEntries)
        {
            var cart = new Dictionary<Product, int>();

            foreach (var cartEntry in cartEntries)
            {
                var product = db.Products.FirstOrDefault(entry => entry.Id == cartEntry.Key);
                if (product != null && cartEntry.Value > 0)
                    cart.Add(product, cartEntry.Value);
            }
            Session["Cart"] = cart;
            return RedirectToAction("Cart", "Products");
        }

        public ActionResult Cart()
        {
            if (Session["Cart"] != null)
            {
                var order = new Order();
                order.Products = Session["Cart"] as Dictionary<Product, int>;
                var siteOwner = SubdomainHelper.GetSiteOwner();
                var orderHint = string.Format(@Resources.labels.OrderShippingCondition, siteOwner.Country,
                                              siteOwner.DeliveryService, siteOwner.User.UserName);
                return View(order).Warning(orderHint);
            }
            return RedirectToAction("Index", "Home").Warning(Resources.labels.CartEmpty);
        }

        public ActionResult CreateOrder()
        {
            if (Session["Cart"] != null)
            {
                var order = new Order();
                order.Products = Session["Cart"] as Dictionary<Product, int>;
                var siteOwner = SubdomainHelper.GetSiteOwner();
                var orderHint = string.Format(@Resources.labels.OrderShippingCondition, siteOwner.Country,
                                              siteOwner.DeliveryService, siteOwner.User.UserName);
                return View(order).Warning(orderHint);
            }
            return RedirectToAction("Index", "Home").Warning(Resources.labels.CartEmpty);
        }

        [HttpPost]
        public ActionResult CreateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                order.Products = Session["Cart"] as Dictionary<Product, int>;
                EmailHelper.Instance.SendOrderCreated(order);
                return RedirectToAction("Index", "Home").Warning(Resources.labels.OrderCreated);
            }

            return View(order);
        }
    }
}