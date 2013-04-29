using System.Threading;

namespace Internet.Models
{
    public partial class Result
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
    }
}