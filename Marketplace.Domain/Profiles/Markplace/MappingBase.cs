namespace Marketplace.Domain.Profiles.Markplace
{
    public class MappingBase : AutoMapper.Profile
    {
        public MappingBase()
        {
            AllowNullDestinationValues = false;
            AllowNullCollections = false;
            CreateMap<Entities.shared.BaseAddress, Models.dto.location.Address>();
            CreateMap<Models.dto.location.Address, Entities.shared.BaseAddress>();
        }
    }
}
