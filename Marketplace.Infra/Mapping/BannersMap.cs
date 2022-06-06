using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Mapping
{
    public class BannersMap
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Banner> builder)
        {
            builder.ToTable("banners");
            builder.HasKey(prop => prop.id);
        }
    }
}