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

            builder.HasMany(h => h.Logs)
                   .WithOne(w => w.Appointment)
                   .HasForeignKey(f => f.appointment_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Assessments)
                   .WithOne(w => w.Appointment)
                   .HasForeignKey(f => f.appointment_id)
                   .OnDelete(DeleteBehavior.Cascade);
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
