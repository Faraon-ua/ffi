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
    public class ResultsController : Controller
    {
        private ffiEntities db = new ffiEntities();

        //
        // GET: /Admin/Results/

        public ViewResult Index()
        {
            return View(db.Results.ToList());
        }

        //
        // GET: /Admin/Results/Details/5

        public ViewResult Details(Guid id)
        {
            Result result = db.Results.Single(r => r.Id == id);
            return View(result);
        }

        //
        // GET: /Admin/Results/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/Results/Create

        [HttpPost]
        public ActionResult Create(Result result)
        {
            if (ModelState.IsValid)
            {
                result.Id = Guid.NewGuid();
                db.Results.AddObject(result);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(result);
        }
        
        //
        // GET: /Admin/Results/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Result result = db.Results.Single(r => r.Id == id);
            return View(result);
        }

        //
        // POST: /Admin/Results/Edit/5

        [HttpPost]
        public ActionResult Edit(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Attach(result);
                db.ObjectStateManager.ChangeObjectState(result, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(result);
        }

        //
        // GET: /Admin/Results/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Result result = db.Results.Single(r => r.Id == id);
            return View(result);
        }

        //
        // POST: /Admin/Results/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Result result = db.Results.Single(r => r.Id == id);
            db.Results.DeleteObject(result);
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