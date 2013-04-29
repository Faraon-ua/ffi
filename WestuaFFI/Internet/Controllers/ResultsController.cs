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
    public class ResultsController : Controller
    {
        private ffiEntities db = new ffiEntities();

        //
        // GET: /Results/

        public class ResultsModelView
        {
            private static ffiEntities db = new ffiEntities();

            public List<string> Manufacturers = db.Results.Select(entry => entry.Manufacturer).ToList();
            public List<string> Models = new List<string>();
            public Result Result { get; set; }
        }

        public ViewResult Index(string manufacturer, string model)
        {
            var resultView = new ResultsModelView();            
            if (!string.IsNullOrEmpty(manufacturer))
            {
                resultView.Models =
                    db.Results.Where(entry => entry.Manufacturer == manufacturer).Select(entry => entry.Model).ToList();
                if (!string.IsNullOrEmpty(model))
                    resultView.Result =
                        db.Results.FirstOrDefault(entry => entry.Manufacturer == manufacturer && entry.Model == model);
            }
            return View(resultView);
        }

        //
        // GET: /Results/Details/5

        public ViewResult Details(Guid id)
        {
            Result result = db.Results.Single(r => r.Id == id);
            return View(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}