using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.account.provider
{
    public class accountProviderRs
    {
        public Response.provider.providerRs provider { get; set; }
        public List<Response.provider.providerAppointment> appointments { get; set; }
        public List<Response.provider.providerScheduleRs> schedules { get; set; }
        public List<Entities.Topic> topics { get; set; }
        public List<banks.bankRs> banks { get; set; }
        public List<Domain.Models.dto.Item> accounttypes { get; set; }
        public List<Entities.Language> languages { get; set; }
        public appointment.appointmentRs appointment { get; set; } = null;
    }
}
