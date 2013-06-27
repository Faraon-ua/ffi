using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Internet.Models;

namespace Internet.Controllers
{ 
    public class FAQsController : Controller
    {
        private ffiEntities db = new ffiEntities();

        //
        // GET: /FAQs/

        public ViewResult Index()
        {
            return View(db.FAQs.Where(entry=>entry.ShowOnPartnerPanel == false).OrderBy(entry=>entry.Question_ru).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}