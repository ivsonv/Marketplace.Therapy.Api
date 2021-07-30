using System.Collections.Generic;
namespace Marketplace.Domain.Models.dto.customer
{
    public class customerDto
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string cnpj { get; set; }
        public string cpf { get; set; }
        public string image { get; set; }
        public bool? active { get; set; }
        public bool? newsletter { get; set; }

        public List<location.Address> address { get; set; }
    }
}
