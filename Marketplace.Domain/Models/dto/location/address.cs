namespace Marketplace.Domain.Models.dto.location
{
    public class Address
    {
        public int? id { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string neighborhood { get; set; }
        public string complement { get; set; }
        public string number { get; set; }
        public string zipcode { get; set; }
        public string uf { get; set; }
        public string country { get; set; }
    }
}
