using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.account.provider
{
    public class accountProviderRs
    {
        public Response.provider.providerRs Provider { get; set; }
        public List<Entities.Topic> topics { get; set; }
        public List<Entities.Bank> banks { get; set; }
        public List<Entities.Language> languages { get; set; }
    }
}
