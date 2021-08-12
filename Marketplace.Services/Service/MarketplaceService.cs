using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Models.Request.marketplace;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class MarketplaceService
    {
        private readonly ICustomCache _cache;

        public MarketplaceService(ICustomCache cache) 
        {
            _cache = cache;
        }

        public async Task ShowProviders(providerRq _request)
        {
            try
            {
                var list = await _cache.GetProviders();


            }
            catch (System.Exception ex) { /*_res.setError(ex);*/ }
            //return _res;
        }
    }
}