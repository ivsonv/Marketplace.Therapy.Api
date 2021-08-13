using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Models.Request;
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

        public async Task<BaseRs<List<providerMktRs>>> ShowProviders(BaseRq<providerMktRq> _request)
        {
            var _res = new BaseRs<List<providerMktRs>>();
            if (_request.data == null)
                _request.data = new providerMktRq();
            try
            {
                var list = await _cache.GetProviders();

                if (_request.data.name.IsNotEmpty())
                    list = list.Where(w => w.fantasy_name.IsCompare().Contains(_request.data.name.IsCompare()) ||
                                           w.company_name.IsCompare().Contains(_request.data.name.IsCompare()) ||
                                           w.nickname.IsCompare().Contains(_request.data.name.IsCompare())
                                           ).ToList();

                // list
                _request.pagination.size = 20; //force
                _res.content = list
                    .OrderBy(o => o.fantasy_name)
                    .Skip(_request.pagination.size * _request.pagination.page)
                    .Take(_request.pagination.size)
                    .Select(x => new providerMktRs()
                    {
                        name = x.nickname.IsNotEmpty() ? x.nickname : $"{x.fantasy_name} {x.company_name}",
                        image = x.image.toImageUrl(_configuration["storage:image"]),
                        price = x.price.ToString(),
                        biography = x.biography,
                        crp = x.crp
                    }).ToList();
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}