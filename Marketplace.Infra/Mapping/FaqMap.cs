using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class FaqMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Faq> builder)
        {
            builder.ToTable("faq");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.title).HasColumnType("varchar(120)");
            builder.Property(prop => prop.sub_title).HasColumnType("varchar(120)");

            builder.HasMany(h => h.Question)
                   .WithOne(w => w.Faq)
                   .HasForeignKey(f => f.faq_id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FaqQuestionMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.FaqQuestion> builder)
        {
            builder.ToTable("faq_questions");
            builder.HasKey(prop => prop.id);
        }
    }
}
