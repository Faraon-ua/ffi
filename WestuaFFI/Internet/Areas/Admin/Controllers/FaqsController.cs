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
    public class FaqsController : Controller
    {
        private ffiEntities db = new ffiEntities();

        //
        // GET: /Admin/Faqs/

        public ViewResult Index()
        {
            return View(db.FAQs.ToList());
        }

        //
        // GET: /Admin/Faqs/Details/5

        public ViewResult Details(Guid id)
        {
            FAQ faq = db.FAQs.Single(f => f.Id == id);
            return View(faq);
        }

        //
        // GET: /Admin/Faqs/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/Faqs/Create

        [HttpPost]
        public ActionResult Create(FAQ faq)
        {
            if (ModelState.IsValid)
            {
                faq.Id = Guid.NewGuid();
                db.FAQs.AddObject(faq);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(faq);
        }
        
        //
        // GET: /Admin/Faqs/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            FAQ faq = db.FAQs.Single(f => f.Id == id);
            return View(faq);
        }

        //
        // POST: /Admin/Faqs/Edit/5

        [HttpPost]
        public ActionResult Edit(FAQ faq)
        {
            if (ModelState.IsValid)
            {
                db.FAQs.Attach(faq);
                db.ObjectStateManager.ChangeObjectState(faq, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(faq);
        }

        //
        // GET: /Admin/Faqs/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            FAQ faq = db.FAQs.Single(f => f.Id == id);
            return View(faq);
        }

        //
        // POST: /Admin/Faqs/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            FAQ faq = db.FAQs.Single(f => f.Id == id);
            db.FAQs.DeleteObject(faq);
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