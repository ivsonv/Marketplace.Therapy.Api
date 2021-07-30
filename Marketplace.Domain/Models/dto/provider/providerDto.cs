using System.Collections.Generic;
namespace Marketplace.Domain.Models.dto.company
{
    public class providerDto
    {
        public int? id { get; set; }
        public string fantasy_name { get; set; } = null;
        public string company_name { get; set; } = null;
        public string email { get; set; } = null;
        public string phone { get; set; } = null;
        public string password { get; set; } = null;
        public string cnpj { get; set; } = null;
        public string cpf { get; set; } = null;
        public string image { get; set; } = null;
        public Helpers.Enumerados.ProviderStatus? situation { get; set; } = null;

        public List<location.Address> address { get; set; } = null;
    }
}
