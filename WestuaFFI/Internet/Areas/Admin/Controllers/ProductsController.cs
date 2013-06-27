using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Internet.Models;

namespace Internet.Areas.Admin.Controllers
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
        // GET: /Admin/Products/

        public ViewResult Index()
        {
            return View(db.Products.ToList());
        }

        //
        // GET: /Admin/Products/Details/5

        public ViewResult Details(Guid id)
        {
            Product product = db.Products.Single(p => p.Id == id);
            return View(product);
        }

        //
        // GET: /Admin/Products/Create

        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.OrderBy(entry => entry.Index).ToList();
            return View();
        }

        //
        // POST: /Admin/Products/Create

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase productImg)
        {
            if (ModelState.IsValid)
            {
                if (productImg != null)
                    using (var ms = new MemoryStream())
                    {
                        productImg.InputStream.CopyTo(ms);
                        byte[] imgArray = ms.GetBuffer();
                        product.Image = new WebImage(imgArray).Resize(200, 200).Crop(1,1).GetBytes("png");
                    }

                product.Id = Guid.NewGuid();
                db.Products.AddObject(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        //
        // GET: /Admin/Products/Edit/5
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ViewBag.Categories = db.Categories.OrderBy(entry => entry.Index).ToList();
            Product product = db.Products.Single(p => p.Id == id);
            return View(product);
        }

        //
        // POST: /Admin/Products/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase productImg)
        {
            if (ModelState.IsValid)
            {
                var oldProduct = db.Products.FirstOrDefault(entry => entry.Id == product.Id);
                if (productImg != null)
                    using (var ms = new MemoryStream())
                    {
                        productImg.InputStream.CopyTo(ms);
                        byte[] imgArray = ms.GetBuffer();
                        product.Image = new WebImage(imgArray).Resize(200, 200).Crop(1, 1).GetBytes("image/png");
                    }
                else
                    product.Image = oldProduct.Image;
                db.Products.Detach(oldProduct);
                db.Products.Attach(product);
                db.ObjectStateManager.ChangeObjectState(product, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Admin/Products/Delete/5

        public ActionResult Delete(Guid id)
        {
            Product product = db.Products.Single(p => p.Id == id);
            return View(product);
        }

        //
        // POST: /Admin/Products/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Product product = db.Products.Single(p => p.Id == id);
            db.Products.DeleteObject(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}