using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class LanguageMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Language> builder)
        {
            builder.ToTable("languages");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.name).HasColumnType("varchar(120)");

            builder.HasMany(h => h.ProviderLanguages)
                   .WithOne(w => w.Language)
                   .HasForeignKey(f => f.language_id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
