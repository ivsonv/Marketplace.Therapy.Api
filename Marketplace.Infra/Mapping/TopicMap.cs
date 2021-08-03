using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class TopicMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Topic> builder)
        {
            builder.ToTable("topics");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.name).HasColumnType("varchar(120)");

            builder.HasMany(h => h.ProviderTopics)
                   .WithOne(w => w.Topic)
                   .HasForeignKey(f => f.topic_id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
