using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.company
{
    public class providerRs
    {
        public providerRs()
        {
            this.company = new List<dto.company.providerDto>();
        }

        public List<dto.company.providerDto> company { get; set; }
    }
}
