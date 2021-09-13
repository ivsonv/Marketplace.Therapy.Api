using Marketplace.Domain.Helpers;
using System;

namespace Marketplace.Domain.Models.Request.payment
{
    public class paymentRq
    {
        public Enumerados.PaymentMethod payment_method { get; set; }
        public DateTime date { get; set; }
        public TimeSpan hour { get; set; }
        public int provider_id { get; set; }
    }
}
