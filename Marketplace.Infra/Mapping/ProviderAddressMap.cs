using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class ProviderAddressMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ProviderAddress> builder)
        {
            builder.ToTable("providers_address");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.country).HasColumnType("varchar(2)");
            builder.Property(prop => prop.number).HasColumnType("varchar(10)");
            builder.Property(prop => prop.uf).HasColumnType("varchar(2)");
            builder.Property(prop => prop.zipcode).HasColumnType("varchar(12)");
        }
    }
}
