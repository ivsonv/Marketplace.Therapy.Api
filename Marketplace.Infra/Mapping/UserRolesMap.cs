using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class UserRolesMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.UserRoles> builder)
        {
            builder.ToTable("users_roles");
            builder.HasKey(prop => prop.id);
        }
    }
}