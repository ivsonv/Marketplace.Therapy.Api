using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.provider
{
    public class providerRs
    {
        public providerRs()
        {
            this.provider = new List<dto.provider.providerDto>();
        }

        public List<dto.provider.providerDto> provider { get; set; }
    }
}
