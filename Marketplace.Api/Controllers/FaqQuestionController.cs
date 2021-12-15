using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Response;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [CustomAuthorizePermission]
    [Route("api/faq/question")]
    public class FaqQuestionController : DefaultController
    {
        private readonly FaqQuestionService _faqQuestionService;
        public FaqQuestionController(FaqQuestionService faqService)
        {
            _faqQuestionService = faqService;
        }

        [HttpGet]
        public async Task<BaseRs<List<FaqQuestion>>> Show([FromQuery] BaseRq<FaqQuestion> _request)
            => await _faqQuestionService.show(_request);

        [HttpGet("{id:int}")]
        public async Task<BaseRs<FaqQuestion>> FindById([FromRoute] int id)
            => await _faqQuestionService.FindById(id);

        [HttpPost]
        public async Task<BaseRs<FaqQuestion>> Store([FromBody] BaseRq<FaqQuestion> _request)
            => await _faqQuestionService.Store(_request);

        [HttpPut]
        public async Task<BaseRs<FaqQuestion>> Update([FromBody] BaseRq<FaqQuestion> _request)
            => await _faqQuestionService.Update(_request);

        [HttpDelete("{id:int}")]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _faqQuestionService.Delete(id);
    }
}