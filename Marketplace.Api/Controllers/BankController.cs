using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.banks;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.banks;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/banks")]
    public class BankController : DefaultController
    {
        private readonly BankService _bankService;
        public BankController(BankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet]
        public async Task<BaseRs<List<bankRs>>> Show([FromQuery] BaseRq<bankRq> _request)
            => await _bankService.show(_request);

        [HttpGet("{id:int}")]
        public async Task<BaseRs<bankRs>> FindById([FromRoute] int id)
            => await _bankService.FindById(id);

        [HttpPost]
        public async Task<BaseRs<bankRs>> Store([FromBody] BaseRq<bankRq> _request)
            => await _bankService.Store(_request);

        [HttpPut]
        public async Task<BaseRs<bankRs>> Update([FromBody] BaseRq<bankRq> _request)
            => await _bankService.Update(_request);

        [HttpDelete("{id:int}")]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _bankService.Delete(id);

        [HttpGet("account-types")]
        public dynamic ShowSituations() => _bankService.getAccountTypes();
    }
}