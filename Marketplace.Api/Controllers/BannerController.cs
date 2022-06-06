using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [CustomAuthorizePermission]
    [Route("api/banner")]
    public class BannerController : DefaultController
    {
        private readonly BannerService _bannerService;
        public BannerController(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<BaseRs<List<Domain.Entities.Banner>>> Show([FromQuery] BaseRq<string> _request) 
            => await _bannerService.Show(new BaseRq<Domain.Entities.Banner>());


        [HttpPost]
        public async Task<BaseRs<Domain.Entities.Banner>> Store([FromBody] BaseRq<Domain.Entities.Banner> _request)
            => await _bannerService.Store(_request);

        [HttpPut]
        public async Task<BaseRs<Domain.Entities.Banner>> Update([FromBody] BaseRq<Domain.Entities.Banner> _request)
            => await _bannerService.Update(_request);

        [HttpGet("{id:int}")]
        public async Task<BaseRs<Domain.Entities.Banner>> FindById([FromRoute] int id)
            => await _bannerService.FindById(id);

        [HttpGet("situations")]
        public dynamic ShowSituations()
            => _bannerService.getSituations();
    }
}