using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class CustomerMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Customer> builder)
        {
            builder.ToTable("customers");

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
        }
    }

    public class CustomerAddressMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.CustomerAddress> builder)
        {
            builder.ToTable("customers_address");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.country).HasColumnType("varchar(2)");
            builder.Property(prop => prop.number).HasColumnType("varchar(10)");
            builder.Property(prop => prop.uf).HasColumnType("varchar(2)");
            builder.Property(prop => prop.zipcode).HasColumnType("varchar(12)");
        }
    }
}
