using System.Collections.Generic;

namespace Marketplace.Integrations.Payment.Nexxera.repository
{
    public class ConsultRs : NXerrors
    {
        public ConsultPaymentRs payment { get; set; }
    }
    public class ConsultPaymentRs
    {
        public string transactionType { get; set; }
        public string paymentStatus { get; set; }
        public string paymentToken { get; set; }
        public int amount { get; set; }
    }
}