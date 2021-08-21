using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.account.provider
{
    public class accountProviderRs
    {
        public Response.provider.providerRs provider { get; set; }
        public List<Entities.Topic> topics { get; set; }
        public List<banks.bankRs> banks { get; set; }
        public List<Entities.Language> languages { get; set; }
    }
}
