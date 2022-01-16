using System.Collections.Generic;

namespace Marketplace.Domain.Models.Request.marketplace
{
    public class providerMktRq
    {
        public string name { get; set; }
        public List<string> provs { get; set; }
    }
}