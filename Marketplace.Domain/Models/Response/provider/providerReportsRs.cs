using System;
using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.provider
{
    public class providerReportsRs
    {
        public List<providerReport> reports { get; set; }
    }

    public class providerReport
    {
        public int id { get; set; }
        public decimal revenue { get; set; }
        public string start { get; set; }
        public string hour { get; set; }
        public string customer { get; set; }
    }
}