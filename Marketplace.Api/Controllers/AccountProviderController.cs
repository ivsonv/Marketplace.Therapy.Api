using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.account.provider;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
    [Route("api/account-provider")]
    public class AccountProviderController : DefaultController
    {
        private readonly AccountProviderService _account;
        public AccountProviderController(AccountProviderService accountProviderService)
        {
            _account = accountProviderService;
        }

        [HttpGet]
        public async Task<BaseRs<accountProviderRs>> FindBy()
            => await _account.findByUser();

        //[HttpPut]
        //public async Task<BaseRs<providerRs>> Update([FromBody] BaseRq<providerRq> _request)
        //{
        //    if (_request.data.id != base.MyUser.id)
        //        return new BaseRs<providerRs>() { error = new BaseError(new List<string>() { "Solicitação inválida." }) };

        //    return await _providerService.Update(_request);
        //}

        //[HttpGet("situations")]
        //public dynamic ShowSituations()
        //    => _providerService.getSituations();

        //[HttpGet("banks")]
        //public dynamic ShowBanks()
        //   => _providerService.getSituations();
    }
}