using System;
using System.Data;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Internet.Extensions;
using Internet.Models;

namespace Internet.Controllers
{
    public class BannersController : Controller
    {

        private ffiEntities db = new ffiEntities();

        public ActionResult GetImage(Guid id)
        {
            var banner = db.Banners.FirstOrDefault(entry => entry.Id == id);
            var arr = banner.Image == null ? new byte[0] : banner.Image;
            return File(arr, "image/png");
        }

        [Authorize]
        public ActionResult Update(Banner banner, HttpPostedFileBase bannerImg)
        {
            var partner = db.Partners.FirstOrDefault(entry => entry.Id == banner.PartnerId);

            if (string.IsNullOrEmpty(banner.Url) || bannerImg == null) return RedirectToAction("Edit", "Partners", new { id = partner.User.UserName }).Warning((string.Format("{0} {1}", "Banner", Resources.labels.Incorrect)));
            if (partner != null)
            {
                if (bannerImg != null)
                    using (var ms = new MemoryStream())
                    {
                        bannerImg.InputStream.CopyTo(ms);
                        byte[] imgArray = ms.GetBuffer();
                        banner.Image = new WebImage(imgArray).Resize(450, 70).Crop(1, 1).GetBytes("png");
                    }
                if (partner.Banners.Any())
                {
                    var oldBanner = db.Banners.FirstOrDefault(entry => entry.Id == banner.Id);
                    oldBanner.Url = banner.Url;
                    oldBanner.Image = banner.Image;
                }
                else
                {
                    banner.Id = Guid.NewGuid();
                    db.Banners.AddObject(banner);
                }
                db.SaveChanges();
                return RedirectToAction("Edit", "Partners", new { id = partner.User.UserName }).Warning(@Resources.labels.AccountUpdated);
            }
            return RedirectToAction("Edit", "Partners", new { id = partner.User.UserName }).Warning("Error, please contact administrator");
        }

        public ActionResult Remove(Guid id)
        {
            var banner = db.Banners.FirstOrDefault(entry => entry.Id == id);
            var userName = banner.Partner.User.UserName;
            db.Banners.DeleteObject(banner);
            db.SaveChanges();
            return RedirectToAction("Edit", "Partners", new { id = userName }).Warning(@Resources.labels.AccountUpdated);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
