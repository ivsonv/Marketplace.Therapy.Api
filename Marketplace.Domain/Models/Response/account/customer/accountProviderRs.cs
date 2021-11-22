using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.account.customer
{
    public class accountCustomerRs
    {
        public Response.customers.customerRs customer { get; set; } = null;
        public List<CustomerAppointment> appointments { get; set; } = null;
        public appointment.appointmentRs appointment { get; set; } = null;
    }
    public class CustomerAppointment
    {
        public string provider_name { get; set; } = null;
        public string data { get; set; } = null;
        public string hora { get; set; } = null;
        public string dsStatus { get; set; } = null;
        public string issued { get; set; }
        public string transaction_code { get; set; }
        public int id { get; set; }
    }
}
