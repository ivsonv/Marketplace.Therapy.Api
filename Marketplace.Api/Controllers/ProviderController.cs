using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.company;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.company;
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

        [HttpGet]
        public async Task<BaseRs<providerRs>> Show([FromQuery] BaseRq<providerRq> _request)
            => await _providerService.Show(_request);

        [HttpPost]
        public async Task<BaseRs<providerRs>> Store([FromBody] BaseRq<providerRq> _request)
            => await _providerService.Store(_request);

        //[HttpGet("{id:int}")]
        //public async Task<BaseRs<customerRs>> FindById([FromRoute] int id)
        //    => await _customerService.FindById(id);

        //[HttpPut]
        //public async Task<BaseRs<customerRs>> Update([FromBody] BaseRq<customerRq> _request)
        //    => await _customerService.Update(_request);

        //[HttpDelete("{id:int}")]
        //public async Task<BaseRs<bool>> Delete([FromRoute] int id)
        //    => await _customerService.Delete(id);
    }
}