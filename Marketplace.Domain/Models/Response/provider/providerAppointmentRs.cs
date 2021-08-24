using System;
using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.provider
{
    public class providerAppointmentRs
    {
        public List<providerAppointment> appointments { get; set; }
    }

    public class providerAppointment
    {
        public int id { get; set; }
        public decimal revenue { get; set; }
        public TimeSpan hour { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int provider_id { get; set; }

        public Helpers.Enumerados.AppointmentType type { get; set; }
        public customerAppointment customer { get; set; }
    }

    public class customerAppointment
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}