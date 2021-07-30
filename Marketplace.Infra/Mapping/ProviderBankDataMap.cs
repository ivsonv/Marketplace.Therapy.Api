using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class ProviderBankAccountMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ProviderBankAccount> builder)
        {
            builder.ToTable("providers_bank_account");
            builder.Property(prop => prop.agency_number).HasColumnType("varchar(20)");
            builder.Property(prop => prop.agency_digit).HasColumnType("varchar(3)");
            builder.Property(prop => prop.account_number).HasColumnType("varchar(20)");
            builder.Property(prop => prop.account_digit).HasColumnType("varchar(3)");
            builder.Property(prop => prop.bank_code).HasColumnType("varchar(10)");
            builder.HasKey(prop => prop.id);
        }
    }
}
