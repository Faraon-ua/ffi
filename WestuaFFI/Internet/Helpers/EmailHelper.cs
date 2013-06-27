using System;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using Internet.Models;
using Internet.RoutingHelpers;

namespace Internet.Helpers
{
    public class EmailHelper
    {
        private const string From = "info@ffi-eco.com";
        private const string ToAdmin = "info@ffi-eco.com";
        private const string Header = "FFI Ecology";

        private static EmailHelper _instance; 
        public static EmailHelper Instance
        {
            get { return _instance ?? (_instance = new EmailHelper()); }
        }

        public void SendResultContactOwner(string video, string sendTo, string contacts)
        {
            var body = string.Format(Resources.Emails.ResultContactOwner, video, contacts);
            var subject = "Client request result / Запрос клиента о результате / Запит клієнта про результ";
            SendEmail(From, sendTo, body, subject);
        }

        public void SendNewResult(Result result)
        {
            SendEmail(From, ToAdmin, string.Format("http://ffi-eco.com/Admin/Results/Edit/{0}", result.Id),
                      "New Result Added");
        }

        public void SendPasswordRecovery(string to, string password)
        {
            SendEmail(From, to, string.Format("Your new password is: {0}", password), "Your new password");
        }

        public void SendPaymentRecieved(Partner partner, int monthExtended)
        {
            var body = string.Format(Resources.Emails.AccountEpirationExtended, partner.Name, monthExtended,
                                     partner.ExpirationDate.ToString("dd.MM.yyyy"));
            SendEmail(From, partner.ContactsEmail, body, "Your account updated");
        }

        public void SendUniqEmailRequest(string emailName, string replyTo)
        {
            var body = string.Format(Resources.Emails.UniqEmailRequest, emailName, replyTo);
            SendEmail(From, ToAdmin, body, "New uniq email request");
        }

        public void SendOrderCreated(Order order)
        {
            var siteOwner = SubdomainHelper.GetSiteOwner();
            SendEmail(From, order.Email, Resources.Emails.OrderCreatedClientText, Resources.Emails.OrderCreatedClientSubject);
            var products = new StringBuilder();
            foreach (var product in order.Products)
            {
                products.Append(product.Value + "x" + product.Key.Name+"<br/>");
            }
            var text = string.Format(Resources.Emails.OrderCreatedText, order.Name, order.Email, order.Phone,
                                     order.Address, products, order.Total);
            SendEmail(From, siteOwner.ContactsEmail, text , "New Order from site FFI-ECO.COM");
        }

        public void SendContactsEmail(string toAddress, string name, string email, string text)
        {
            SendEmail(From, toAddress, string.Format("Name: {0}<br/>Email: {1}<br/>Message: {2}", name, email, text),
                      "New contacts question from site");
        }

        public void SendConfirmationEmail(string userName)
        {
            var user = Membership.GetUser(userName);
            var confirmationGuid = user.ProviderUserKey.ToString();
            var verifyUrl = HttpContext.Current.Request.Url.GetLeftPart
               (UriPartial.Authority) + "/Account/Verify/" + confirmationGuid;
 
            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(From, user.Email))
                {
                    message.Subject = "Please Verify your Account";
                    message.Body = string.Format(Resources.Emails.AccountConfirmationText, user.UserName, verifyUrl);
 
                    message.IsBodyHtml = true;
                    
                    client.Send(message);
                };
            };
        }

        public bool NewUserAlert(string userName)
        {
            return SendEmail(From, ToAdmin, string.Format(Resources.Emails.NewUserBody, userName), "New user registered");
        }

        public bool UserActivated(string userName)
        {
            var user = Membership.GetUser(userName);
            if (user != null)
            {
                return SendEmail(From, user.Email, Resources.Emails.AccountActivated, Resources.Emails.AccountActivatedHeader);
            }
            return false;
        }

        private bool SendEmail(string fromad, string toad, string body, string subjectcontent)
        {
            bool result;
            MailMessage usermail = Mailbodplain(fromad, toad, body, Header, subjectcontent);
            SmtpClient client = new SmtpClient();

            try
            {
                client.Send(usermail);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            } 

            return result;

        }

        private MailMessage Mailbodplain(string fromad, string toad, string body, string header, string subjectcontent)
        {
            var mail = new MailMessage();
            string from = fromad;
            string to = toad;
            mail.To.Add(to);
            mail.From = new MailAddress(from, header, System.Text.Encoding.UTF8);
            mail.Subject = subjectcontent;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            return mail;
        }

    }
}