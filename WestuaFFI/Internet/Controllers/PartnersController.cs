using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Internet.Extensions;
using Internet.Models;

namespace Internet.Controllers
{
    [Authorize]
    public class PartnersController : Controller
    {
        public static readonly Dictionary<int, int> ExtendAmounts = new Dictionary<int, int> { { 1, 50 }, { 2, 100 }, { 3, 150 }, { 6, 250 }, { 12, 500 } };

        private int _amount;
        public int Amount
        {
            get
            {
                if (ExtendAmounts.Values.Contains(_amount))
                    return _amount;
                return -1;
            }
            set { _amount = value; }
        }

        private ffiEntities db = new ffiEntities();

        //
        // GET: /Partners/Create
        public ActionResult Create()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = Membership.GetUser(userName);
            var userGuid = new Guid(user.ProviderUserKey.ToString());
            if (db.Partners.FirstOrDefault(entry => entry.UserId == userGuid) == null)
            {
                var partner = new Partner { UserId = userGuid };
                return View(partner);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Partners/Create

        [HttpPost]
        public ActionResult Create(Partner partner)
        {
            if (ModelState.IsValid && db.Partners.FirstOrDefault(entry => entry.UserId == partner.UserId) == null)
            {
                partner.Id = Guid.NewGuid();
                partner.ExpirationDate = DateTime.Now;
                db.Partners.AddObject(partner);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = User.Identity.Name }).Warning(Resources.labels.AccountPartnershipCreated);
            }

            return View(partner);
        }

        //
        // GET: /Partners/Edit/5

        public ActionResult Edit(string id)
        {
            var user = Membership.GetUser(id);
            if (user != null)
            {
                var userGuid = new Guid(user.ProviderUserKey.ToString());
                var partner = db.Partners.SingleOrDefault(p => p.UserId == userGuid);
                if (partner == null) return RedirectToAction("Create").Warning(Resources.labels.PartnerNotExist);
                return View(partner);
            }
            return null;
        }

        //
        // POST: /Partners/Edit/5

        [HttpPost]
        public ActionResult Edit(Partner partner)
        {
            if (ModelState.IsValid)
            {
                db.Partners.Attach(partner);
                db.ObjectStateManager.ChangeObjectState(partner, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Edit", new { userName = User.Identity.Name }).Warning(Resources.labels.AccountUpdated);
            }
            return View(partner);
        }

        public ActionResult ExtendExpirationDate(int? extendValue)
        {
            if (extendValue.HasValue)
            {
                Amount = extendValue.Value;
            }
            return View(Amount);
        }

        public int AddPayment(int amount, string userName)
        {
            var user = Membership.GetUser(userName);
            if (user != null)
            {
                var userGuid = new Guid(user.ProviderUserKey.ToString());
                var partner = db.Partners.SingleOrDefault(entry => entry.UserId == userGuid);
                if (partner != null)
                {
                    var maxOrdeId = db.Payments.Any() ? db.Payments.Max(entry => entry.OrderId) : 0;
                    var payment = new Payment
                        {
                            Id = Guid.NewGuid(),
                            Amount = amount,
                            Completed = false,
                            CreatedAt = DateTime.Now,
                            OrderId = maxOrdeId + 1,
                            PaymentFor = "Account"
                        };
                    partner.Payments.Add(payment);
                    db.SaveChanges();
                    return payment.OrderId;
                }
            }
            return -1;
        }

//        public int VerifyPayment(int amount, string userName)
//        {
//
//        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}