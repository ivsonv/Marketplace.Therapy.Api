using System.Collections.Generic;

namespace Marketplace.Domain.Profiles.Markplace
{
    public class MappingCustomers : AutoMapper.Profile
    {
        public MappingCustomers()
        {
            AllowNullDestinationValues = false;
            AllowNullCollections = false;

            CreateMap<Entities.Customer, Models.dto.customer.customerDto>()
                .ForMember(d => d.address,
                                o => o.MapFrom(s => s.Address.ConvertAll(cc =>
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

            CreateMap<List<Entities.CustomerAddress>, List<Models.dto.location.Address>>();
            CreateMap<Entities.CustomerAddress, Entities.shared.BaseAddress>();

            CreateMap<Models.dto.customer.customerDto, Entities.Customer>()
                     .ForMember(d => d.Address,
                                o => o.MapFrom(s => s.address.ConvertAll(cc =>
                                                    new Entities.CustomerAddress()
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
            CreateMap<List<Models.dto.location.Address>, List<Entities.CustomerAddress>>();
            CreateMap<Entities.shared.BaseAddress, Entities.CustomerAddress>();
        }
    }
}
