using Marketplace.Domain.Models.dto.provider;
using System;
using System.Collections.Generic;

namespace Marketplace.Domain.Models.dto.payment
{
    public class PaymentDto
    {
        public List<PaymentList> payments { get; set; }
        public Helpers.Enumerados.PaymentProvider PaymentProvider { get; set; }
    }

    public class PaymentList
    {
        public Helpers.Enumerados.PaymentMethod PaymentMethod { get; set; }

        public customer.customerDto Customer { get; set; }
        public providerDto Provider { get; set; }
        public ProductSale productSale { get; set; }
        public double totalprice { get; set; }
        public CardDto card { get; set; }
        public Helpers.Enumerados.PaymentStatus paymentStatus { get; set; }
        public Helpers.Enumerados.AppointmentStatus status { get; set; }
        public string LogRq { get; set; }
        public string LogRs { get; set; }
        public string webhook_url { get; set; }
        public string transactionUrl { get; set; }
        public string transactionCode { get; set; }
        public bool cancel { get; set; }
    }

    public class ProductSale
    {
        public int id { get; set; }
        public double price { get; set; }
        public DateTime Data { get; set; }
    }
}