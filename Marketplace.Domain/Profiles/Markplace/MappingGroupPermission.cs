using System.Collections.Generic;

namespace Marketplace.Domain.Profiles.Markplace
{
    public class MappingGroupPermission : AutoMapper.Profile
    {
        public MappingGroupPermission()
        {
            AllowNullDestinationValues = false;
            AllowNullCollections = false;

            CreateMap<Entities.GroupPermission, Models.Response.groupPermissions.groupPermissionRs>();
        }
    }
}
