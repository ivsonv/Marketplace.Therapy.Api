using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Models.Request.marketplace;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.marketplace;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class MarketplaceService
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomCache _cache;

        public MarketplaceService(IConfiguration configuration, ICustomCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<BaseRs<List<providerRs>>> ShowProviders(providerRq _request)
        {
            var _res = new BaseRs<List<providerRs>>();
            try
            {
                var list = await _cache.GetProviders();

                if (_request.name.IsNotEmpty())
                    list = list.Where(w => w.fantasy_name.Clear().Contains(_request.name.Clear()) ||
                                           w.company_name.Clear().Contains(_request.name.Clear()) ||
                                           w.nickname.Clear().Contains(_request.name.Clear())
                                           ).ToList();

                // list
                _res.content = list.ConvertAll(x => new providerRs()
                {
                    name = x.nickname.IsNotEmpty() ? x.nickname : $"{x.fantasy_name} {x.company_name}",
                    image = x.image.toImageUrl(_configuration["storage:image"]),
                    price = x.price.ToString(),
                    biography = x.biography,
                    crp = x.crp
                });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}