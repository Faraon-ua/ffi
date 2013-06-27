using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using Internet.Models;

namespace Internet.Helpers
{

    public class CurrencyHelper
    {
        private const string ApiRoute = "https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=5";
        private const string CurrencyRateKey = "CurrencyRate";

        public static CurrencyRate CurrencyRates
        {
            get
            {
                var cache = HttpContext.Current.Cache;
                if (cache[CurrencyRateKey] == null)
                {
                    var xmlDoc = XDocument.Load(ApiRoute, LoadOptions.None);
                    var exchangerates = xmlDoc.Descendants("exchangerate");
                    var currencyRate = new CurrencyRate();
                    currencyRate.UAH = double.Parse(exchangerates.FirstOrDefault(entry => entry.Attribute("ccy").Value == "USD").Attribute("buy").Value, CultureInfo.InvariantCulture);
                    currencyRate.EUR = double.Parse(exchangerates.FirstOrDefault(entry => entry.Attribute("ccy").Value == "EUR").Attribute("buy").Value, CultureInfo.InvariantCulture) / currencyRate.UAH;
                    currencyRate.RUB = double.Parse(exchangerates.FirstOrDefault(entry => entry.Attribute("ccy").Value == "RUR").Attribute("buy").Value, CultureInfo.InvariantCulture) / currencyRate.UAH;
                    cache.Insert(CurrencyRateKey, currencyRate, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60));
                }
                return cache[CurrencyRateKey] as CurrencyRate;
            }
        }


        public static int RoundToTens(double value)
        {
            return ((int)(value / 10)) * 10;
        }
    }
}