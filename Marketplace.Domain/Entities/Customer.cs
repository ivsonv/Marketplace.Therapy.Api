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

    public class CustomerAddress : shared.BaseAddress
    {
        public int customer_id { get; set; }
        public Customer Customer { get; set; }
    }

    public class CustomerAssessment : BaseEntity
    {
        public int sale_id { get; set; }
        public int customer_id { get; set; }
        public double stars { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
    }
}