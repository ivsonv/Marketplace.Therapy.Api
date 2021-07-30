using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class ProviderMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Provider> builder)
        {
            builder.ToTable("providers");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.company_name).HasColumnType("varchar(255)");
            builder.Property(prop => prop.fantasy_name).HasColumnType("varchar(255)");
            builder.Property(prop => prop.email).HasColumnType("varchar(150)");
            builder.Property(prop => prop.cpf).HasColumnType("varchar(11)");
            builder.Property(prop => prop.cnpj).HasColumnType("varchar(14)");
            builder.Property(prop => prop.image).HasColumnType("varchar(50)");
            builder.Property(prop => prop.phone).HasColumnType("varchar(255)");

            builder.HasMany(h => h.Address)
                   .WithOne(w => w.Provider)
                   .HasForeignKey(f => f.provider_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.BankDatas)
                   .WithOne(w => w.Company)
                   .HasForeignKey(f => f.company_id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
