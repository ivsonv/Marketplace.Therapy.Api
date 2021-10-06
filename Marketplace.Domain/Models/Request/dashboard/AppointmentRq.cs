using System;

namespace Marketplace.Domain.Models.Request.dashboard
{
    public class AppointmentRq
    {
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public Helpers.Enumerados.AppointmentStatus? status { get; set; }
        public Helpers.Enumerados.PaymentStatus? payment_status { get; set; }
        public int provider_id { get; set; }
        public int customer_id { get; set; }
        public string transaction_code { get; set; }
    }
}