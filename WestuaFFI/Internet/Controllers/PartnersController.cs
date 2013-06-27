using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Serialization;
using Internet.Extensions;
using Internet.Helpers;
using Internet.Models;

namespace Internet.Controllers
{
    public class PartnersController : Controller
    {
        public class PartnerViewModel
        {
            public Partner Partner { get; set; }
            public IEnumerable<FAQ> Faqs { get; set; }
        }

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
        [Authorize]
        public ActionResult Create()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = Membership.GetUser(userName);
            var userGuid = new Guid(user.ProviderUserKey.ToString());
            if (db.Partners.FirstOrDefault(entry => entry.UserId == userGuid) == null)
            {
                var partner = new Partner
                    {
                        UserId = userGuid,
                        ContactsEmail = user.Email,
                    };
                return View(partner);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Partners/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(Partner partner)
        {
            if (ModelState.IsValid && db.Partners.FirstOrDefault(entry => entry.UserId == partner.UserId) == null)
            {
                partner.Id = Guid.NewGuid();
                partner.ExpirationDate = DateTime.Now;
                partner.TestDriveHTML = "IwK-oqFFcqU";
                db.Partners.AddObject(partner);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = User.Identity.Name }).Warning(Resources.labels.AccountPartnershipCreated);
            }

            return View(partner);
        }

        //
        // GET: /Partners/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            var user = Membership.GetUser(User.Identity.Name);
            if (user != null)
            {
                var userGuid = new Guid(user.ProviderUserKey.ToString());
                var partner = db.Partners.SingleOrDefault(p => p.UserId == userGuid);
                if (partner == null) return RedirectToAction("Create").Warning(Resources.labels.PartnerNotExist);
                var viewModel = new PartnerViewModel
                    {
                        Partner = partner,
                        Faqs = db.FAQs.Where(entry => entry.ShowOnPartnerPanel == true)
                    };
                return View(viewModel);
            }
            return null;
        }

        //
        // POST: /Partners/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Partner partner)
        {
            if (ModelState.IsValid)
            {
                var oldPartner = db.Partners.FirstOrDefault(entry => entry.Id == partner.Id);
                partner.ExpirationDate = oldPartner.ExpirationDate;
                db.Partners.Detach(oldPartner);
                db.Partners.Attach(partner);
                db.ObjectStateManager.ChangeObjectState(partner, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Edit", new { userName = User.Identity.Name }).Warning(Resources.labels.AccountUpdated);
            }
            var viewModel = new PartnerViewModel
            {
                Partner = partner,
                Faqs = db.FAQs.Where(entry => entry.ShowOnPartnerPanel == true)
            };
            return View(viewModel);
        }

        [Authorize]
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

        [HttpPost]
        [Authorize]
        public ActionResult CreateUniqEmail(string emailName, string replyTo)
        {
            EmailHelper.Instance.SendUniqEmailRequest(emailName, replyTo);
            return RedirectToAction("Edit", "Partners", new { id = User.Identity.Name }).Warning(@Resources.labels.RequestCreated);
        }

        [Authorize]
        public ActionResult UpdatePresentation(Guid partnerId, string presentation)
        {
            var partner = db.Partners.FirstOrDefault(entry => entry.Id == partnerId);
            if (partner != null)
            {
                if (string.IsNullOrEmpty(presentation)) return RedirectToAction("Edit", new { id = partner.User.UserName }).Warning(string.Format("{0} {1} {2}", Resources.labels.TestDrive, "youtube link", Resources.labels.Incorrect));
                var myUri = new Uri(presentation);
                string videoCode = HttpUtility.ParseQueryString(myUri.Query).Get("v");
                partner.TestDriveHTML = videoCode;
                if (string.IsNullOrEmpty(partner.TestDriveHTML)) return RedirectToAction("Edit", new { id = partner.User.UserName }).Warning(string.Format("{0} {1} {2}", Resources.labels.TestDrive, "youtube link", Resources.labels.Incorrect));
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = partner.User.UserName }).Warning(@Resources.labels.AccountUpdated);
            }
            return RedirectToAction("Edit", new { id = partner.User.UserName }).Warning("Error, please contact administrator");
        }

        [Authorize]
        public ActionResult UpdateResult(Result result)
        {
            if (ModelState.IsValid)
            {
//                if (string.IsNullOrEmpty(result.VideoTag)) return RedirectToAction("Edit", new { id = partner.User.UserName }).Warning(string.Format("{0} {1} {2}", Resources.labels.TestDrive, "youtube link", Resources.labels.Incorrect));
                result.Id = Guid.NewGuid();
                result.isActive = false;
                var myUri = new Uri(result.VideoTag);
                string videoCode = HttpUtility.ParseQueryString(myUri.Query).Get("v");
                result.VideoTag = videoCode;
                db.Results.AddObject(result);
                db.SaveChanges();
                EmailHelper.Instance.SendNewResult(result);
            }
            return RedirectToAction("Edit", new { id = User.Identity.Name }).Warning(Resources.labels.AccountUpdated);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}