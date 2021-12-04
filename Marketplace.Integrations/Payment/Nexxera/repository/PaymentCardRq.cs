namespace Marketplace.Integrations.Payment.Nexxera.repository
{
    public class PaymentCardRq
    {
        public int installments { get; set; }
        public bool capture { get; set; }
        public string returnUrl { get; set; }
        public int transactionType { get; set; }
        public string merchantOrderId { get; set; }
        public string amount { get; set; }
        public string callbackUrl { get; set; }

        public CardToken cardToken { get; set; }
        public Card Card { get; set; }
        public PaymentCardCustomer customer { get; set; }
    }

    public class PaymentCardCustomer
    {
        public string tag { get; set; }
        public string name { get; set; }
        public string identity { get; set; }
        public string identityType { get; set; }
        public string email { get; set; }

        public CardHolderAddress address { get; set; }

    }

    public class CardToken
    {
        public string token { get; set; }
        public string securityCode { get; set; }
    }

    public class Card
    {
        public string number { get; set; }
        public string securityCode { get; set; }
        public bool saveCard { get; set; }
        public int? brand { get; set; }
        public CardExpirationDate expirationDate { get; set; }
        public CardHolder holder { get; set; }
    }

    public class CardExpirationDate
    {
        public string year { get; set; }
        public string month { get; set; }
    }

    public class CardHolder
    {
        public string name { get; set; }
        public string socialNumber { get; set; }

        public CardHolderAddress billingAddress { get; set; }
    }

    public class CardHolderAddress
    {
        public string country { get; set; }
        public string zipCode { get; set; }
        public string number { get; set; }
        public string street { get; set; }
        public string complement { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }
}