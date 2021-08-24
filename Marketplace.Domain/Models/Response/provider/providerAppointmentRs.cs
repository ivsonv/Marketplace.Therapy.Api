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
        public int? id { get; set; } = null;
        public decimal? revenue { get; set; } = null;
        public TimeSpan? hour { get; set; } = null;
        public DateTime? start { get; set; } = null;
        public string startds { get; set; } = null;
        public DateTime? end { get; set; } = null;
        public string endds { get; set; } = null;
        public int? provider_id { get; set; } = null;

        public Helpers.Enumerados.AppointmentType type { get; set; }
        public customerAppointment customer { get; set; } = null;
    }

    public class customerAppointment
    {
        public int? id { get; set; } = null;
        public string name { get; set; } = null;
        public string email { get; set; } = null;
    }
}