using System.Configuration;

namespace Internet.Helpers
{
    public class AppSettings
    {
        public static string ShopSecretKey
        {
            get { return ConfigurationManager.AppSettings["ShopSecretKey"]; }
        }

        public static string DomainName
        {
            get { return ConfigurationManager.AppSettings["domainName"]; }
        }
    }
}