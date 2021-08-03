using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class CustomerMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Customer> builder)
        {
            builder.ToTable("customer");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.name).HasColumnType("varchar(120)");
            builder.Property(prop => prop.email).HasColumnType("varchar(150)");
            builder.Property(prop => prop.cpf).HasColumnType("varchar(11)");
            builder.Property(prop => prop.cnpj).HasColumnType("varchar(14)");
            builder.Property(prop => prop.image).HasColumnType("varchar(50)");

            builder.HasMany(h => h.Address)
                   .WithOne(w => w.Customer)
                   .HasForeignKey(f => f.customer_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Assessment)
                   .WithOne(w => w.Customer)
                   .HasForeignKey(f => f.customer_id)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(h => h.Appointments)
                  .WithOne(w => w.Customer)
                  .HasForeignKey(f => f.provider_id)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class CustomerAddressMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.CustomerAddress> builder)
        {
            builder.ToTable("customer_address");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.country).HasColumnType("varchar(2)");
            builder.Property(prop => prop.number).HasColumnType("varchar(10)");
            builder.Property(prop => prop.uf).HasColumnType("varchar(2)");
            builder.Property(prop => prop.zipcode).HasColumnType("varchar(12)");
        }
    }

    public class CustomerAssessmentMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.CustomerAssessment> builder)
        {
            builder.ToTable("customer_assessments");
            builder.HasKey(prop => prop.id);
        }
    }
}
