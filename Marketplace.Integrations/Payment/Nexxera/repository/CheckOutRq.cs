using System.Collections.Generic;

namespace Marketplace.Integrations.Payment.Nexxera.repository
{
    public class CheckOutRq
    {
        public int installments { get; set; }
        public string returnUrl { get; set; }
        public List<string> items { get; set; }
        public string merchantOrderId { get; set; }
        public CheckOutCustomer customer { get; set; }
        public string amount { get; set; }
        public CheckOutcreditCard creditCard { get; set; }
        public CheckOutdebitCard debitCard { get; set; }
    }

    public class CheckOutCustomer
    {
        public string name { get; set; }
        public string identity { get; set; }
        public string identityType { get; set; }
        public string email { get; set; }
        public string birthdate { get; set; }
    }

    public class CheckOutcreditCard
    {
        public CheckOutcreditCard()
        {
            this.installments = new List<CheckOutcreditCardinstallments>();
        }
        public List<CheckOutcreditCardinstallments> installments { get; set; }
    }

    public class CheckOutcreditCardinstallments
    {
        public int Number { get; set; }
        public string amount { get; set; }
        public string template { get; set; }
    }

    public class CheckOutdebitCard
    {
        public int amount { get; set; }
    }
}
