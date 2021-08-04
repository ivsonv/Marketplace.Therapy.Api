using System;

namespace Marketplace.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public decimal price_commission { get; set; }
        public decimal price_cost { get; set; }
        public decimal price_full { get; set; }
        public decimal price { get; set; }
        public DateTime booking_date { get; set; }
        public Helpers.Enumerados.AppointmentOrigin origin { get; set; }
        public Helpers.Enumerados.PaymentStatus payment_status { get; set; }
        public Helpers.Enumerados.AppointmentStatus status { get; set; }

        public int customer_id { get; set; }
        public int provider_id { get; set; }

        public Provider Provider { get; set; }
        public Customer Customer { get; set; }
    }
}