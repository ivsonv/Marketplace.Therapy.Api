using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Domain.Profiles.Markplace
{
    public class MappingProvider : AutoMapper.Profile
    {
        public MappingProvider()
        {
            AllowNullDestinationValues = false;
            AllowNullCollections = false;

            CreateMap<Entities.Provider, Models.dto.provider.providerDto>()
                .ForMember(d => d.address,
                                o => o.MapFrom(s => s.Address.Select(cc =>
                                                    new Models.dto.location.Address
                                                    {
                                                        address = cc.address,
                                                        city = cc.city,
                                                        complement = cc.complement,
                                                        uf = cc.uf,
                                                        country = cc.country,
                                                        neighborhood = cc.neighborhood,
                                                        number = cc.number,
                                                        zipcode = cc.zipcode,
                                                        id = cc.id,
                                                    })));

            CreateMap<List<Entities.ProviderAddress>, List<Models.dto.location.Address>>();
            CreateMap<Entities.ProviderAddress, Entities.shared.BaseAddress>();

            CreateMap<Models.dto.provider.providerDto, Entities.Provider>()
                     .ForMember(d => d.Address,
                                o => o.MapFrom(s => s.address.ConvertAll(cc =>
                                                    new Entities.ProviderAddress()
                                                    {
                                                        address = cc.address,
                                                        city = cc.city,
                                                        complement = cc.complement,
                                                        uf = cc.uf,
                                                        country = cc.country,
                                                        neighborhood = cc.neighborhood,
                                                        number = cc.number,
                                                        id = cc.id ?? 0,
                                                        zipcode = cc.zipcode
                                                    })));
            CreateMap<List<Models.dto.location.Address>, List<Entities.ProviderAddress>>();
            CreateMap<Entities.shared.BaseAddress, Entities.ProviderAddress>();
            CreateMap<Models.dto.provider.providerDto, Models.Request.provider.providerRq>();
            CreateMap<Models.Request.account.provider.accountProviderRq, Models.Request.provider.providerRq>();
        }
    }
}
