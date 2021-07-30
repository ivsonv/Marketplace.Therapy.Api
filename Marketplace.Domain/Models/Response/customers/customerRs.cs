using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.customers
{
    public class customerRs
    {
        public customerRs()
        {
            this.customer = new List<dto.customer.customerDto>();
        }

        public List<dto.customer.customerDto> customer { get; set; }
    }
}
