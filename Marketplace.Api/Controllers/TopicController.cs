using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.topics;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.topics;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/topics")]
    public class TopicController : DefaultController
    {
        private readonly TopicService _topicService;
        public TopicController(TopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<BaseRs<List<topicRs>>> Show([FromQuery] BaseRq<topicRq> _request)
            => await _topicService.show(_request);

        [HttpGet("{id:int}")]
        public async Task<BaseRs<topicRs>> FindById([FromRoute] int id)
            => await _topicService.FindById(id);

        [HttpPost]
        public async Task<BaseRs<topicRs>> Store([FromBody] BaseRq<topicRq> _request)
            => await _topicService.Store(_request);

        [HttpPut]
        public async Task<BaseRs<topicRs>> Update([FromBody] BaseRq<topicRq> _request)
            => await _topicService.Update(_request);

        [HttpDelete("{id:int}")]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _topicService.Delete(id);
    }
}