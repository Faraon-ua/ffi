using System.Collections.Generic;
using System.Threading;

namespace Internet.Models
{
    public partial class Product
    {
        public string Description
        {
            get
            {
                switch (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "en":
                        return Description_en;
                    case "ru":
                        return Description_ru;
                    default:
                        return Description_ua;
                }
            }
        }

        public string Name
        {
            get
            {
                switch (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "en":
                        return Name_en;
                    case "ru":
                        return Name_ru;
                    default:
                        return Name_ua;
                }
            }
        }
    }

    public class ProductEqualityComparer: IEqualityComparer<Product>
    {
        public bool Equals(Product x, Product y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Product obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}