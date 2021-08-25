using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Marketplace.Domain.Helpers.Enumerados;

namespace Marketplace.Domain.Entities
{
    public class Provider : BaseEntity
    {
        public string fantasy_name { get; set; }
        public string company_name { get; set; }
        public string email { get; set; }
        public string link { get; set; }
        public DateTime birthdate { get; set; }
        public string crp { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string cnpj { get; set; }
        public string cpf { get; set; }
        public string image { get; set; }
        public string signature { get; set; }
        public string description { get; set; }
        public string biography { get; set; }
        public string academic_training { get; set; }
        public int interval_between_appointment { get; set; }
        public string nickname { get; set; }
        public decimal price { get; set; }
        public decimal? price_for_thirty { get; set; }
        public string time_zone { get; set; }
        public ProviderStatus situation { get; set; }
        public ProviderGender gender { get; set; }
        public bool active { get; set; }
        public bool remove { get; set; }

        public IEnumerable<ProviderAddress> Address { get; set; }
        public IEnumerable<ProviderBankAccount> BankAccounts { get; set; }
        public IEnumerable<ProviderSplitAccount> SplitAccounts { get; set; }
        public List<ProviderSchedule> Schedules { get; set; }
        public IEnumerable<ProviderCategories> Categories { get; set; }
        public IEnumerable<ProviderTopics> Topics { get; set; }
        public IEnumerable<ProviderLanguages> Languages { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public IEnumerable<ProviderReceipt> ProviderReceipts { get; set; }
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

        [NotMapped]
        public string ds_bank { get; set; }
        public AccountBankType account_bank_type { get; set; }

        public int provider_id { get; set; }
        public Provider Provider { get; set; }
    }

    public class ProviderSplitAccount : BaseEntity
    {
        [Column(TypeName = "jsonb")]
        public string json { get; set; }
        public PaymentProvider payment_provider { get; set; }

        public int provider_id { get; set; }
        public Provider Provider { get; set; }
    }

    public class ProviderSchedule : BaseEntity
    {
        public TimeSpan start { get; set; }
        public TimeSpan end { get; set; }
        public int day_week { get; set; }
        public int provider_id { get; set; }
        public Provider Provider { get; set; }
    }

    public class ProviderCategories : BaseEntity
    {
        public int category_id { get; set; }
        public int provider_id { get; set; }

        public Provider Provider { get; set; }
        public Category Category { get; set; }
    }

    public class ProviderTopics : BaseEntity
    {
        public int topic_id { get; set; }
        public int provider_id { get; set; }

        public Provider Provider { get; set; }
        public Topic Topic { get; set; }
    }

    public class ProviderReceipt : BaseEntity
    {
        public string signature { get; set; }
        public string cpf { get; set; }
        public string fantasy_name { get; set; }
        public string cnpj { get; set; }
        public string address { get; set; }

        public int provider_id { get; set; }
        public Provider Provider { get; set; }
    }

    public class ProviderLanguages : BaseEntity
    {
        public int language_id { get; set; }
        public int provider_id { get; set; }

        public Provider Provider { get; set; }
        public Language Language { get; set; }
    }
}