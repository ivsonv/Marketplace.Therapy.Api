using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        /// <summary>
        /// valor da comissão
        /// </summary>
        public decimal price_commission { get; set; }

        /// <summary>
        /// valor das taxas da operadora
        /// </summary>
        public decimal price_cost { get; set; }

        /// <summary>
        /// preço full do provider
        /// </summary>
        public decimal price_full { get; set; }

        /// <summary>
        /// preço de repasse para o provider (price_full - price_commission)
        /// </summary>
        public decimal price_transfer { get; set; }

        /// <summary>
        /// Preço final para o cliente
        /// </summary>
        public decimal price { get; set; } 
        public DateTime booking_date { get; set; }
        public Helpers.Enumerados.AppointmentType type { get; set; }
        public Helpers.Enumerados.AppointmentOrigin origin { get; set; }
        public Helpers.Enumerados.PaymentStatus payment_status { get; set; }
        public Helpers.Enumerados.AppointmentStatus status { get; set; }
        public string transaction_code { get; set; }

        public int customer_id { get; set; }
        public int provider_id { get; set; }
        public Provider Provider { get; set; }
        public Customer Customer { get; set; }
        public IList<AppointmentLog> Logs { get; set; }
    }

    public class AppointmentLog : BaseEntity
    {
        [Column(TypeName = "jsonb")]
        public string jsonRq { get; set; }

        [Column(TypeName = "jsonb")]
        public string jsonRs { get; set; }
        public string description { get; set; }
        public int appointment_id { get; set; }
        public Appointment Appointment { get; set; }
    }
}