using System.Globalization;
using System.Threading;

namespace Internet.Models
{
    public partial class Category
    {
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
}