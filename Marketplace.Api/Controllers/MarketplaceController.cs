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

        [HttpGet]
        public async Task<BaseRs<List<providerMktRs>>> Show([FromQuery] providerMktRq _request)
            => await _marketplaceService.ShowProviders(_request);

        //[HttpGet("{id:int}")]
        //public async Task<BaseRs<bankRs>> FindById([FromRoute] int id)
        //    => await _bankService.FindById(id);

        //[HttpPost]
        //public async Task<BaseRs<bankRs>> Store([FromBody] BaseRq<bankRq> _request)
        //    => await _bankService.Store(_request);

        //[HttpPut]
        //public async Task<BaseRs<bankRs>> Update([FromBody] BaseRq<bankRq> _request)
        //    => await _bankService.Update(_request);

        //[HttpDelete("{id:int}")]
        //public async Task<BaseRs<bool>> Delete([FromRoute] int id)
        //    => await _bankService.Delete(id);

        //[HttpGet("account-types")]
        //public dynamic ShowSituations() => _bankService.getAccountTypes();
    }
}