using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.account.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.account.provider;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpPut]
        public async Task<BaseRs<accountProviderRs>> UpdateProvider([FromForm] accountProviderRq _reAcc)
            => await _account.updateProvider(_reAcc);

        #region ..: fetchs :..

        [HttpGet]
        public async Task<BaseRs<accountProviderRs>> FindBy()
            => await _account.findByUser();

        [HttpGet("banks")]
        public async Task<BaseRs<accountProviderRs>> GetBanks(string term)
            => await _account.fetchBanks(term);

        [HttpGet("topics")]
        public async Task<BaseRs<accountProviderRs>> GetTopics()
            => await _account.fetchTopics();

        [HttpGet("languages")]
        public async Task<BaseRs<accountProviderRs>> GetLanguages()
            => await _account.fetchLanguages();

        [HttpGet("account-types")]
        public BaseRs<accountProviderRs> GetAccountTypes()
            => _account.fetchAccountTypes();
        #endregion

        #region ..: schedules :..

        [HttpGet("schedules")]
        public async Task<BaseRs<accountProviderRs>> GetSchedules()
            => await _account.fetchSchedules();

        [HttpPost("schedules")]
        public async Task<BaseRs<accountProviderRs>> SaveSchedules([FromBody] BaseRq<accountProviderRq> _request)
            => await _account.SaveSchedule(_request);

        [HttpDelete("schedules/{id:int}")]
        public async Task<BaseRs<bool>> DeleteSchedule([FromRoute] int id)
            => await _account.DeleteSchedule(id);
        #endregion

        #region ..: appointment :..

        [HttpGet("calendar")]
        public async Task<BaseRs<accountProviderRs>> FetchCalendar(int month = -1)
            => await _account.fetchCalendar(month);
        #endregion
    }
}