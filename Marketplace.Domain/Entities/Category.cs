using System.Collections.Generic;

namespace Marketplace.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string description_short { get; set; }
        public string description_long { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public bool active { get; set; }
        public List<ProviderCategories> ProviderCategories { get; set; }
    }
}