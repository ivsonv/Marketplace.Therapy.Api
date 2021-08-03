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

            builder.HasMany(h => h.BankAccounts)
                   .WithOne(w => w.Provider)
                   .HasForeignKey(f => f.provider_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.SplitAccounts)
                   .WithOne(w => w.Provider)
                   .HasForeignKey(f => f.provider_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Topics)
                   .WithOne(w => w.Provider)
                   .HasForeignKey(f => f.provider_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Categories)
                   .WithOne(w => w.Provider)
                   .HasForeignKey(f => f.provider_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Languages)
                   .WithOne(w => w.Provider)
                   .HasForeignKey(f => f.provider_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Schedules)
                   .WithOne(w => w.Provider)
                   .HasForeignKey(f => f.provider_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Appointments)
                   .WithOne(w => w.Provider)
                   .HasForeignKey(f => f.provider_id)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

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

    public class ProviderCategoriesMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ProviderCategories> builder)
        {
            builder.ToTable("provider_categories");
            builder.HasKey(prop => prop.id);
        }
    }

    public class ProviderLanguagesMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ProviderLanguages> builder)
        {
            builder.ToTable("provider_languages");
            builder.HasKey(prop => prop.id);
        }
    }

    public class ProviderScheduleMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ProviderSchedule> builder)
        {
            builder.ToTable("provider_schedules");
            builder.HasKey(prop => prop.id);
        }
    }

    public class ProviderSplitAccountMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ProviderSplitAccount> builder)
        {
            builder.ToTable("provider_split_account");
            builder.HasKey(prop => prop.id);
        }
    }

    public class ProviderTopicsMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ProviderTopics> builder)
        {
            builder.ToTable("provider_topics");
            builder.HasKey(prop => prop.id);
        }
    }
}