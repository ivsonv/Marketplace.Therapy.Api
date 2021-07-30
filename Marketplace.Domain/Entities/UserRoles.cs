using System.Collections.Generic;

namespace Marketplace.Domain.Entities
{
    public class UserRoles : BaseEntity
    {
        public int? user_id { get; set; }
        public Helpers.Enumerados.UserRule role { get; set; }
        public User User { get; set; }
    }
}
