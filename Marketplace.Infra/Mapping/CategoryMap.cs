using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class CategoryMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Category> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(prop => prop.id);
            builder.Property(prop => prop.name).HasColumnType("varchar(120)");
            builder.Property(prop => prop.image).HasColumnType("varchar(50)");
        }
    }
}
