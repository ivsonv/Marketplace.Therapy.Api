namespace Marketplace.Domain.Entities
{
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
