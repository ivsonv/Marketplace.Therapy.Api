using System.Collections.Generic;

namespace Marketplace.Domain.Entities
{
    public class Topic : BaseEntity
    {
        public string name { get; set; }
        public bool experience { get; set; }
        public bool active { get; set; }

        public List<ProviderTopics> ProviderTopics { get; set; }
    }
}