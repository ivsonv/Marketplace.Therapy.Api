using System;
using System.Collections.Generic;

namespace Marketplace.Integrations.Payment.Nexxera.repository
{
    public class PaymentCardRs: NXerrors
    {
        public Payment payment { get; set; }
        public string merchantOrderId { get; set; }
    }
    public class BillingAddress
    {
        public string country { get; set; }
        public string zipCode { get; set; }
        public string number { get; set; }
        public string street { get; set; }
        public string complement { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class PaymentCardHolder
    {
        public string name { get; set; }
        public string socialNumber { get; set; }
        public BillingAddress billingAddress { get; set; }
    }

    public class PaymentCardCard
    {
        public string cardNumber { get; set; }
        public string cardBrand { get; set; }
        public PaymentCardHolder holder { get; set; }
    }

    public class Authorization
    {
        public int amount { get; set; }
        public DateTime processedDate { get; set; }
        public string proofOfSale { get; set; }
        public string authorizationCode { get; set; }
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
    }

    public class Capture
    {
        public int amount { get; set; }
        public DateTime processedDate { get; set; }
        public string proofOfSale { get; set; }
        public string authorizationCode { get; set; }
        public string returnCode { get; set; }
    }

    public class Reversal
    {
        public string reason { get; set; }
        public int amount { get; set; }
        public DateTime processedDate { get; set; }
        public string proofOfSale { get; set; }
        public string authorizationCode { get; set; }
        public string returnCode { get; set; }
    }

    public class RecurrencePlan
    {
        public string merchantPlanId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string interval { get; set; }
        public int amount { get; set; }
    }

    public class Customer
    {
        public string tag { get; set; }
        public string name { get; set; }
        public string identity { get; set; }
        public string identityType { get; set; }
        public string email { get; set; }
        public string birthdate { get; set; }
        public Address address { get; set; }
    }

    public class Payment
    {
        public PaymentCardCard card { get; set; }
        public string transactionType { get; set; }
        public string paymentStatus { get; set; }
        public Authorization authorization { get; set; }
        public List<Capture> captures { get; set; }
        public List<Reversal> reversals { get; set; }
        public string authenticationUrl { get; set; }
        public string cardToken { get; set; }
        public string paymentToken { get; set; }
        public RecurrencePlan recurrencePlan { get; set; }
        public Customer customer { get; set; }
        public int amount { get; set; }
    }
}