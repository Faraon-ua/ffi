using System.Threading;

namespace Internet.Models
{
    public partial class FAQ
    {
        public string Question
        {
            get
            {
                switch (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "en":
                        return Question_en;
                    case "ru":
                        return Question_ru;
                    default:
                        return Question_ua;
                }
            }
        }

        public string Answer
        {
            get
            {
                switch (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "en":
                        return Answer_en;
                    case "ru":
                        return Answer_ru;
                    default:
                        return Answer_ua;
                }
            }
        }
    }
}