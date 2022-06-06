using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.marketplace;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.banners;
using Marketplace.Domain.Models.Response.marketplace;
using Marketplace.Domain.Models.Response.topics;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/marketplace")]
    public class MarketplaceController : DefaultController
    {
        private readonly MarketplaceService _marketplaceService;
        private readonly TopicService _topicService;
        private readonly BannerService _bannerService;


        public MarketplaceController(MarketplaceService marketplaceService,
                                     BannerService bannerService,
                                     TopicService topicService)
        {
            _marketplaceService = marketplaceService;
            _bannerService = bannerService;
            _topicService = topicService;
        }

        [HttpGet("topics")]
        public async Task<BaseRs<List<topicRs>>> Show()
        {
            return await _topicService.showCache(new BaseRq<Domain.Models.Request.topics.topicRq>()
            {
                pagination = new Domain.Models.Pagination()
                {
                    size = 9999,
                    page = 0
                }
            });
        }

        [HttpGet("banners")]
        public async Task<List<bannersRs>> ShowBanners()
        {
            var banners = await _bannerService.All();
            return banners.content.ConvertAll(x => new bannersRs()
            {
                image = x.imageurl,
                type = (int)x.type
            });
        }

        [HttpPost("providers")]
        public async Task<BaseRs<List<providerMktRs>>> ShowProviders([FromBody] BaseRq<providerMktRq> _request)
            => await _marketplaceService.ShowProviders(_request);

        [HttpGet("provider/{linkpermanent}")]
        public async Task<BaseRs<providerMktRs>> Show([FromRoute] string linkpermanent)
            => await _marketplaceService.FindByProvider(linkpermanent);

        [HttpGet("provider/{linkpermanent}/hours")]
        public async Task<BaseRs<providerMktRs>> ShowHours([FromRoute] string linkpermanent, [FromQuery] string dt_start) 
            => await _marketplaceService.AvailableHours(linkpermanent, dt_start.toConvertDate());

        [HttpGet("faq")]
        public async Task<dynamic> ShowFaq()
            => await _marketplaceService.ShowFaq();
    }
}