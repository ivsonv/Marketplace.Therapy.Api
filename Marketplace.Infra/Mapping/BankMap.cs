using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class BankMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Bank> builder)
        {
            builder.ToTable("banks");
            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.name).HasColumnType("varchar(200)");
            builder.Property(prop => prop.code).HasColumnType("varchar(3)");
        }
    }
}
