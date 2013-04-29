using System;
using System.Net.Mail;
using System.Web;
using System.Web.Security;

namespace Internet.Helpers
{
    public class EmailHelper
    {
        private const string From = "info@ffi-eco.com";
        private const string ToAdmin = "ffi.eco.com@gmail.com";
        private const string Header = "FFI Ecology";

        private static EmailHelper _instance; 
        public static EmailHelper Instance
        {
            get { return _instance ?? (_instance = new EmailHelper()); }
        }

        public void SendContactsEmail(string name, string email, string text)
        {
            SendEmail(From, ToAdmin, string.Format("Name: {0}, Email: {1}<br/>{2}", name, email, text),
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
                    message.Body = string.Format(Resources.labels.AccountConfirmationText, user.UserName, verifyUrl);
 
                    message.IsBodyHtml = true;
                    
                    client.EnableSsl = true;
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

            client.EnableSsl = true;
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