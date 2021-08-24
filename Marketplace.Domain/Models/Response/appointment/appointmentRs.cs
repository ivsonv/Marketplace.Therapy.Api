using System;

namespace Marketplace.Domain.Models.Response.appointment
{
    public class appointmentRs : Entities.Appointment
    {
        public string start { get; set; }
        public TimeSpan hour { get; set; }
    }
}