using System.Collections.Generic;

namespace Marketplace.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string cnpj { get; set; }
        public string cpf { get; set; }
        public string image { get; set; }
        public bool newsletter { get; set; }
        public bool active { get; set; }
        public string recoverpassword { get; set; }
        public int recoverqtd { get; set; }

        public List<CustomerAddress> Address { get; set; }
    }
}
