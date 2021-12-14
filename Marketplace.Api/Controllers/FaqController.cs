using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.topics;
using Marketplace.Domain.Models.Response;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [CustomAuthorizePermission]
    [Route("api/faq")]
    public class FaqController : DefaultController
    {
        private readonly FaqService _faqService;
        public FaqController(FaqService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet]
        public async Task<BaseRs<List<Faq>>> Show([FromQuery] BaseRq<Faq> _request)
            => await _faqService.show(_request);

        [HttpGet("{id:int}")]
        public async Task<BaseRs<Faq>> FindById([FromRoute] int id)
            => await _faqService.FindById(id);

        [HttpPost]
        public async Task<BaseRs<Faq>> Store([FromBody] BaseRq<Faq> _request)
            => await _faqService.Store(_request);

        [HttpPut]
        public async Task<BaseRs<Faq>> Update([FromBody] BaseRq<Faq> _request)
            => await _faqService.Update(_request);

        [HttpDelete("{id:int}")]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _faqService.Delete(id);
    }
}