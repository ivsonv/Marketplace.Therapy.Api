using System.Collections.Generic;

namespace Marketplace.Domain.Models.Request.Categories
{
    public class categoryRq
    {
        public string name { get; set; }
        public int id { get; set; }
        public List<Subcategory> subcategories { get; set; }
    }
    public class Subcategory
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
