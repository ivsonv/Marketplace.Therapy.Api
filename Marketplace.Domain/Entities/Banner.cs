using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Domain.Entities
{
    public class Banner : BaseEntity
    {
        [NotMapped]
        public string imageurl { get; set; }
        public string image { get; set; }
        public Helpers.Enumerados.BannerType type { get; set; }

        [NotMapped]
        public string ds_type { get; set; }

    }
}