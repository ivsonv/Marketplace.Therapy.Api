using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.account.customer;
using Marketplace.Domain.Models.Request.account.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.account.customer;
using Marketplace.Domain.Models.Response.account.provider;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/account-customer")]
    public class AccountCustomerController : DefaultController
    {
        private readonly AccountCustomerService _account;
        public AccountCustomerController(AccountCustomerService accountcustomerService)
        {
            _account = accountcustomerService;
        }

        [HttpPost]
        public async Task<BaseRs<accountCustomerRs>> StoreCustomer([FromBody] accountCustomerRq _reAcc)
            => await _account.storeCustomer(_reAcc);

        //[HttpPut, CustomAuthorizePermission(Permissions = permission.Account.ViewCustomer)]
        //public async Task<BaseRs<accountCustomerRs>> UpdateProvider([FromForm] accountCustomerRq _reAcc)
        //    => await _account.updateProvider(_reAcc);
    }
}