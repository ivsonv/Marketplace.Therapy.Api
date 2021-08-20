using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
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

        [HttpGet, CustomAuthorizePermission(Permissions = permission.Bank.View)]
        public async Task<BaseRs<List<bankRs>>> Show([FromQuery] BaseRq<bankRq> _request)
            => await _bankService.show(_request);

        [HttpGet("{id:int}"), CustomAuthorizePermission(Permissions = permission.Bank.View)]
        public async Task<BaseRs<bankRs>> FindById([FromRoute] int id)
            => await _bankService.FindById(id);

        [HttpPost, CustomAuthorizePermission(Permissions = permission.Bank.Create)]
        public async Task<BaseRs<bankRs>> Store([FromBody] BaseRq<bankRq> _request)
            => await _bankService.Store(_request);

        [HttpPut, CustomAuthorizePermission(Permissions = permission.Bank.Edit)]
        public async Task<BaseRs<bankRs>> Update([FromBody] BaseRq<bankRq> _request)
            => await _bankService.Update(_request);

        [HttpDelete("{id:int}"), CustomAuthorizePermission(Permissions = permission.Bank.Delete)]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _bankService.Delete(id);

        [HttpGet("account-types"), CustomAuthorizePermission(Permissions = permission.Bank.View)]
        public dynamic ShowSituations() => _bankService.getAccountTypes();
    }
}