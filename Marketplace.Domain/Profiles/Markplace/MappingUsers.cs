using System.Collections.Generic;

namespace Marketplace.Domain.Profiles.Markplace
{
    public class MappingUsers : AutoMapper.Profile
    {
        public MappingUsers()
        {
            AllowNullDestinationValues = false;
            AllowNullCollections = false;

            CreateMap<Entities.User, Models.Response.users.userRs>();
        }
    }
}
