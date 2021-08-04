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
        public double totalprice { get; set; }
        public CardDto card { get; set; }
    }
}
