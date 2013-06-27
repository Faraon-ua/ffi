using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Xml.Serialization;
using Internet.Helpers;
using Internet.Models;

namespace Internet.Controllers
{
    public class PaymentsController : Controller
    {
        private ffiEntities db = new ffiEntities();

        public void Verify(string merchant_id, int order_id, string payment_id, string desc, string payment_type, string amount, string commission, string sign)
        {
            var payment1 = new EasyPayPayment
            {
                merchant_id = merchant_id,
                order_id = order_id,
                payment_id = payment_id,
                desc = desc,
                payment_type = payment_type,
                amount = amount,
                commission = commission,
                sign = sign
            };

            var payment2 = new EasyPayPayment
            {
                merchant_id = Request["merchant_id"],
                order_id = int.Parse(Request["order_id"]),
                payment_id = Request["payment_id"],
                desc = Request["desc"],
                payment_type = Request["payment_type"],
                amount = Request["amount"],
                commission = Request["commission"],
                sign = Request["sign"]
            };


            var sha = SHA256.Create();
            var stringToHash = string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", merchant_id, order_id, payment_id,
                                               desc, payment_type, amount, commission, AppSettings.ShopSecretKey);

            var encoding = new System.Text.UTF8Encoding();
            byte[] messageBytes = encoding.GetBytes(stringToHash);
            byte[] hashmessage = sha.ComputeHash(messageBytes);
            var clientSign = Convert.ToBase64String(hashmessage);

            payment1.client_sign = payment2.client_sign = clientSign;

            var ser = new XmlSerializer(payment1.GetType());
            var fs = System.IO.File.Open(
                                         Server.MapPath("/XML/" + "easypayrequestPARAMS.xml"),
                                         FileMode.OpenOrCreate,
                                         FileAccess.Write,
                                         FileShare.ReadWrite);
            ser.Serialize(fs, payment1);
            fs.Close();

            ser = new XmlSerializer(payment1.GetType());
            fs = System.IO.File.Open(
                                         Server.MapPath("/XML/" + "easypayrequestREQUEST.xml"),
                                         FileMode.OpenOrCreate,
                                         FileAccess.Write,
                                         FileShare.ReadWrite);
            ser.Serialize(fs, payment1);
            fs.Close();

            if (clientSign == sign)
            {
                var payment = db.Payments.FirstOrDefault(entry => entry.OrderId == order_id);
                if (payment != null)
                {
                    payment.Completed = true;
                    var monthsToAdd = PartnersController.ExtendAmounts.FirstOrDefault(entry => entry.Value == payment.Amount).Key;
                    if (payment.Partner.IsActive)
                        payment.Partner.ExpirationDate = payment.Partner.ExpirationDate.AddMonths(monthsToAdd);
                    else
                        payment.Partner.ExpirationDate = DateTime.Now.AddMonths(monthsToAdd);
                    db.SaveChanges();

                    EmailHelper.Instance.SendPaymentRecieved(payment.Partner, monthsToAdd);
                }
            }
        }

        public ActionResult Success()
        {
            return View();
        } 
        
        public ActionResult Failure()
        {
            return View();
        }
    }
}
