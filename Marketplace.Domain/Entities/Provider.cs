using System.Collections.Generic;

namespace Marketplace.Domain.Entities
{
    public class Provider : BaseEntity
    {
        public string fantasy_name { get; set; }
        public string company_name { get; set; }
        public string email { get; set; }
        public string crp { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string cnpj { get; set; }
        public string cpf { get; set; }
        public string image { get; set; }
        public Helpers.Enumerados.ProviderStatus situation { get; set; }

        public bool remove { get; set; }

        public List<ProviderAddress> Address { get; set; }
        public List<ProviderBankAccount> BankDatas { get; set; }
    }

    public class ProviderAddress : shared.BaseAddress
    {
        public int provider_id { get; set; }
        public Provider Provider { get; set; }
    }

    public class ProviderBankAccount : BaseEntity
    {
        public string agency_number { get; set; }
        public string agency_digit { get; set; }
        public string account_digit { get; set; }
        public string account_number { get; set; }
        public string bank_code { get; set; }

        public int company_id { get; set; }
        public Provider Company { get; set; }
    }
}