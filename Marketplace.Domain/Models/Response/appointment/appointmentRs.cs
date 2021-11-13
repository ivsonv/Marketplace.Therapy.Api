using System;

namespace Marketplace.Domain.Models.Response.appointment
{
    public class appointmentRs : Entities.Appointment
    {
        public string start { get; set; }
        public TimeSpan hour { get; set; }
        public string dsStatus { get; set; }
        public string dsStatusPayment { get; set; }
        public string issued { get; set; }
        public string room_id { get; set; }
        public string room_name { get; set; }
        public int room_traveled { get; set; }
    }
}