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
            builder.Property(prop => prop.name).HasColumnType("varchar(200)");

            builder.HasMany(h => h.GroupPermissions)
                   .WithOne(w => w.User)
                   .HasForeignKey(f => f.user_id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class UserGroupPermissionMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.UserGroupPermission> builder)
        {
            builder.ToTable("user_group_permissions");
            builder.HasKey(prop => prop.id);
        }
    }
}