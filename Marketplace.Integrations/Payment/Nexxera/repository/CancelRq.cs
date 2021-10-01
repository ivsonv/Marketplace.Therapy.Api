using System.Collections.Generic;

namespace Marketplace.Integrations.Payment.Nexxera.repository
{
    public class CancelRq
    {
        public CancelCardPaymentChange CardPaymentChange { get; set; }
    }

    public class CancelCardPaymentChange
    {
        public string paymentToken { get; set; }
        public int amount { get; set; }

    }
}
