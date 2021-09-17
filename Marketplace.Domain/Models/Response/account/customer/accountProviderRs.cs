using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.account.customer
{
    public class accountCustomerRs
    {
        public Response.customers.customerRs customer { get; set; } = null;
        public List<CustomerAppointment> appointments { get; set; } = null;
    }
    public class CustomerAppointment
    {
        public string provider_name { get; set; }
        public string data { get; set; }
        public string hora { get; set; }
        public string dsStatus { get; set; }
        public int id { get; set; }
    }
}
