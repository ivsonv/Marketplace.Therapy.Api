﻿using System.Collections.Generic;

namespace Marketplace.Domain.Entities
{
    public class GroupPermission : BaseEntity
    {
        public string name { get; set; }
        public IEnumerable<GroupPermissionAttached> PermissionsAttached { get; set; }
        public IEnumerable<UserGroupPermission> Users { get; set; }
    }

    public class GroupPermissionAttached : BaseEntity
    {
        public string name { get; set; }
        public int group_permission_id { get; set; }
        public GroupPermission GroupPermission { get; set; }
    }
}