using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.languages;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.languages;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [CustomAuthorizePermission]
    [Route("api/languages")]
    public class LanguageController : DefaultController
    {
        private readonly LanguageService _languageService;
        public LanguageController(LanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        public async Task<BaseRs<List<languageRs>>> Show([FromQuery] BaseRq<languageRq> _request)
            => await _languageService.showCache(_request);

        [HttpGet("{id:int}")]
        public async Task<BaseRs<languageRs>> FindById([FromRoute] int id)
            => await _languageService.FindById(id);

        [HttpPost]
        public async Task<BaseRs<languageRs>> Store([FromBody] BaseRq<languageRq> _request)
            => await _languageService.Store(_request);

        [HttpPut]
        public async Task<BaseRs<languageRs>> Update([FromBody] BaseRq<languageRq> _request)
            => await _languageService.Update(_request);

        [HttpDelete("{id:int}")]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _languageService.Delete(id);
    }
}