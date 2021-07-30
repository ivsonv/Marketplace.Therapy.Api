using System.Collections.Generic;

namespace Marketplace.Domain.Entities
{
    public class User : BaseEntity
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string image { get; set; }
        public bool active { get; set; }

        public List<UserRoles> Roles { get; set; }
    }
}
