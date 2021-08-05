using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class GroupPermissionMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.GroupPermission> builder)
        {
            builder.ToTable("group_permissions");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.name).HasColumnType("varchar(50)");

            builder.HasMany(h => h.PermissionsAttached)
                   .WithOne(w => w.GroupPermission)
                   .HasForeignKey(f => f.group_permission_id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Users)
                   .WithOne(w => w.GroupPermission)
                   .HasForeignKey(f => f.group_permission_id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class GroupPermissionAttachedMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.GroupPermissionAttached> builder)
        {
            builder.ToTable("group_permissions_attached");
            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.name).HasColumnType("varchar(120)");
        }
    }
}