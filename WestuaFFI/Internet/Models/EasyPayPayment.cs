using System;

namespace Internet.Models
{
    [Serializable]
    public class EasyPayPayment
    {
        public string merchant_id { get; set; }
        public int order_id { get; set; }
        public string payment_id { get; set; }
        public string desc { get; set; }
        public string payment_type { get; set; }
        public string amount { get; set; } 
        public string commission { get; set; }
        public string sign { get; set; }
        public string client_sign { get; set; }
    }
}