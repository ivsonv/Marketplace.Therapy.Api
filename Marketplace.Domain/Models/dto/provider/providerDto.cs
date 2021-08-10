using System.Collections.Generic;
namespace Marketplace.Domain.Models.dto.provider
{
    public class providerDto
    {
        public int? id { get; set; }

        public string nickname { get; set; } = null;
        public string fantasy_name { get; set; } = null;
        public string company_name { get; set; } = null;
        public string email { get; set; } = null;
        public string phone { get; set; } = null;
        public string password { get; set; } = null;
        public string cnpj { get; set; } = null;
        public string cpf { get; set; } = null;
        public string image { get; set; } = null;
        public string crp { get; set; } = null;
        public string description { get; set; } = null;
        public string curriculum { get; set; } = null;
        public string biography { get; set; } = null;
        public string academic_training { get; set; } = null;
        public int? interval_between_appointment { get; set; } = null;
        public string ds_situation { get; set; }

        public bool active { get; set; }
        public bool remove { get; set; }
        public Helpers.Enumerados.ProviderStatus? situation { get; set; } = null;
        public List<location.Address> address { get; set; } = null;

        public List<Entities.ProviderBankAccount> bankAccounts { get; set; } = null;
    }
}
