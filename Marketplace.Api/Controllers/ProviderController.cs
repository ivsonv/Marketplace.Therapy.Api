using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.provider;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/provider")]
    public class ProviderController : DefaultController
    {
        private readonly ProviderService _providerService;
        public ProviderController(ProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet, CustomAuthorizePermission]
        public async Task<BaseRs<providerRs>> Show([FromQuery] BaseRq<providerRq> _request)
            => await _providerService.Show(_request);

        //[HttpPost]
        //public async Task<BaseRs<providerRs>> Store([FromBody] BaseRq<providerRq> _request)
        //    => await _providerService.Store(_request);

        [HttpPut, CustomAuthorizePermission]
        public async Task<BaseRs<providerRs>> Update([FromBody] BaseRq<providerRq> _request)
            => await _providerService.Update(_request);

        [HttpGet("{id:int}"), CustomAuthorizePermission]
        public async Task<BaseRs<providerRs>> FindById([FromRoute] int id)
            => await _providerService.FindById(id);

        [HttpGet("situations")]
        public dynamic ShowSituations()
            => _providerService.getSituations();
    }
}