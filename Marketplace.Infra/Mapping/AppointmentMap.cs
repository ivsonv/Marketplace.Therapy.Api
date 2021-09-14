using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class AppointmentMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Appointment> builder)
        {
            builder.ToTable("appointments");
            builder.HasKey(prop => prop.id);
        }
    }

    public class AppointmentLogMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.AppointmentLog> builder)
        {
            builder.ToTable("appointments_logs");
            builder.HasKey(prop => prop.id);
        }
    }
}
