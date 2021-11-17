using System;
using System.Collections.Generic;

namespace Marketplace.Integrations.Payment.Nexxera.repository
{
    public class CancelRs : NXerrors
    {
        public CancelPaymentRs payment { get; set; }
    }
    
    public class CancelPaymentRs
    {
        public CancelCard card { get; set; }
        public int transactionType { get; set; }
        public int paymentStatus { get; set; }
        public CancelAuthorization authorization { get; set; }
        public List<CancelReversal> reversals { get; set; }
        public CancelCustomer customer { get; set; }
    }

    public class CancelCard
    {
        public string cardNumber { get; set; }
        public string cardBrand { get; set; }
        public object holder { get; set; }
    }

    public class CancelAuthorization
    {
        public int amount { get; set; }
        public DateTime processedDate { get; set; }
        public string proofOfSale { get; set; }
        public string authorizationCode { get; set; }
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
        public string providerReturnCode { get; set; }
    }

    public class CancelReversal
    {
        public string reason { get; set; }
        public int amount { get; set; }
        public DateTime processedDate { get; set; }
        public string proofOfSale { get; set; }
        public string authorizationCode { get; set; }
        public string returnCode { get; set; }
        public string providerReturnCode { get; set; }
    }

    public class CancelAddress
    {
        public object country { get; set; }
        public object zipCode { get; set; }
        public object number { get; set; }
        public object street { get; set; }
        public object complement { get; set; }
        public object city { get; set; }
        public object state { get; set; }
        public object neighborhood { get; set; }
    }

    public class CancelCustomer
    {
        public object tag { get; set; }
        public string name { get; set; }
        public string identity { get; set; }
        public string identityType { get; set; }
        public string email { get; set; }
        public object birthdate { get; set; }
        public Address address { get; set; }
    }

}