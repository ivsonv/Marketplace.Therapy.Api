using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class UserMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.name).HasColumnType("varchar(120)");
            builder.Property(prop => prop.email).HasColumnType("varchar(120)");

            builder.HasMany(h => h.Roles)
                   .WithOne(w => w.User)
                   .HasForeignKey(f => f.user_id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
