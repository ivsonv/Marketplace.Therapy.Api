using System.Collections.Generic;

namespace Marketplace.Domain.Entities
{
    public class User : BaseEntity
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
        public IEnumerable<UserGroupPermission> GroupPermissions { get; set; }
    }

    public class UserGroupPermission : BaseEntity
    {
        public int user_id { get; set; }
        public int group_permission_id { get; set; }

        public User User { get; set; }
        public GroupPermission GroupPermission { get; set; }
    }
}