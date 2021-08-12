using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.provider;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/merchant")]
    public class MerchantController : DefaultController
    {
        private readonly MerchantService _merchantService;
        public MerchantController(MerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpPost]
        public async Task<BaseRs<providerRs>> Store([FromBody] BaseRq<providerRq> _request)
            => await _merchantService.Store(_request);
    }
}