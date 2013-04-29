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
    public class SlidesController : Controller
    {
        private ffiEntities db = new ffiEntities();


        public ActionResult GetImage(Guid slideId)
        {
            var slide = db.Slides.FirstOrDefault(entry => entry.Id == slideId);
            var arr = slide.Image == null ? new byte[0] : slide.Image;
            return File(arr, "image/png");
        }
 
        //
        // GET: /Admin/Slides/

        public ViewResult Index()
        {
            return View(db.Slides.ToList());
        }

        //
        // GET: /Admin/Slides/Details/5

        public ViewResult Details(Guid id)
        {
            Slide slide = db.Slides.Single(s => s.Id == id);
            return View(slide);
        }

        //
        // GET: /Admin/Slides/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/Slides/Create

        [HttpPost]
        public ActionResult Create(Slide slide, HttpPostedFileBase slideImg)
        {
            if (ModelState.IsValid)
            {
                if (slideImg != null)
                    using (var ms = new MemoryStream())
                    {
                        slideImg.InputStream.CopyTo(ms);
                        byte[] imgArray = ms.GetBuffer();
                        slide.Image = imgArray; //new WebImage(imgArray).Resize(200, 200).GetBytes("png");
                    }

 
                slide.Id = Guid.NewGuid();
                db.Slides.AddObject(slide);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(slide);
        }
        
        //
        // GET: /Admin/Slides/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Slide slide = db.Slides.Single(s => s.Id == id);
            return View(slide);
        }

        //
        // POST: /Admin/Slides/Edit/5

        [HttpPost]
        public ActionResult Edit(Slide slide, HttpPostedFileBase slideImg)
        {
            if (ModelState.IsValid)
            {
                if (slideImg != null)
                    using (var ms = new MemoryStream())
                    {
                        slideImg.InputStream.CopyTo(ms);
                        byte[] imgArray = ms.GetBuffer();
                        slide.Image = imgArray; //new WebImage(imgArray).Resize(200, 200).GetBytes("png");
                    }

 
                db.Slides.Attach(slide);
                db.ObjectStateManager.ChangeObjectState(slide, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slide);
        }

        //
        // GET: /Admin/Slides/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Slide slide = db.Slides.Single(s => s.Id == id);
            return View(slide);
        }

        //
        // POST: /Admin/Slides/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Slide slide = db.Slides.Single(s => s.Id == id);
            db.Slides.DeleteObject(slide);
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