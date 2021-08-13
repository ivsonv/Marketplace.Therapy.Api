using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.marketplace;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.marketplace;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/marketplace")]
    public class MarketplaceController : DefaultController
    {
        private readonly MarketplaceService _marketplaceService;
        public MarketplaceController(MarketplaceService marketplaceService)
        {
            _marketplaceService = marketplaceService;
        }

        [HttpGet("providers")]
        public async Task<BaseRs<List<providerMktRs>>> ShowProviders([FromQuery] BaseRq<providerMktRq> _request)
            => await _marketplaceService.ShowProviders(_request);

        [HttpGet("provider/{linkpermanent}")]
        public async Task<BaseRs<List<providerMktRs>>> Show([FromRoute] string linkpermanent)
            => await _marketplaceService.ShowProviders(null);
    }
}