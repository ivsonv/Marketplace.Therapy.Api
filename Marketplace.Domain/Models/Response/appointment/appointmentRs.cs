using System;

namespace Marketplace.Domain.Models.Response.appointment
{
    public class appointmentRs : Entities.Appointment
    {
        public string start { get; set; }
        public TimeSpan hour { get; set; }
        public string dsStatus { get; set; }
        public string dsStatusPayment { get; set; }
    }
}