using System;
using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.marketplace
{
    public class providerMktRs
    {
        public string name { get; set; }
        public string image { get; set; }
        public string introduction { get; set; }
        public string biography { get; set; }
        public string price { get; set; }
        public string crp { get; set; }
        public string linkpermanent { get; set; }
        public List<providerMktDate> dates { get; set; }
    }

    public class providerMktDate
    {
        public DateTime date { get; set; }
        public string ds_date { get; set; }
        public List<providerMktDateHour> hours { get; set; }
    }

    public class providerMktDateHour
    {
        public TimeSpan hour { get; set; }
    }
}