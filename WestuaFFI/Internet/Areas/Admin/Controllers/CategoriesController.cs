using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Internet.Models;

namespace Internet.Areas.Admin.Controllers
{ 
    public class CategoriesController : Controller
    {
        private ffiEntities db = new ffiEntities();

        //
        // GET: /Admin/Categories/

        public ViewResult Index()
        {
            return View(db.Categories.OrderBy(entry=>entry.Index).ToList());
        }

        //
        // GET: /Admin/Categories/Details/5

        public ViewResult Details(Guid id)
        {
            Category category = db.Categories.Single(c => c.Id == id);
            return View(category);
        }

        //
        // GET: /Admin/Categories/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/Categories/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                db.Categories.AddObject(category);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(category);
        }
        
        //
        // GET: /Admin/Categories/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Category category = db.Categories.Single(c => c.Id == id);
            return View(category);
        }

        //
        // POST: /Admin/Categories/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Attach(category);
                db.ObjectStateManager.ChangeObjectState(category, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /Admin/Categories/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Category category = db.Categories.Single(c => c.Id == id);
            return View(category);
        }

        //
        // POST: /Admin/Categories/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Category category = db.Categories.Single(c => c.Id == id);
            db.Categories.DeleteObject(category);
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