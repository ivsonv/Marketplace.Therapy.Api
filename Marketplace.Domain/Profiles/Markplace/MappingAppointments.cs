using System.Collections.Generic;

namespace Marketplace.Domain.Profiles.Markplace
{
    public class MappingAppointments : AutoMapper.Profile
    {
        public MappingAppointments()
        {
            AllowNullDestinationValues = false;
            AllowNullCollections = false;

            CreateMap<Entities.Appointment, Models.Response.appointment.appointmentRs>();
            CreateMap<List<Entities.Appointment>, List<Models.Response.appointment.appointmentRs>>();
            CreateMap<Models.Request.appointment.appointmentRq, Models.Response.appointment.appointmentRs>();
            CreateMap<Models.Response.appointment.appointmentRs, Models.Request.appointment.appointmentRq>();
        }
    }
}
