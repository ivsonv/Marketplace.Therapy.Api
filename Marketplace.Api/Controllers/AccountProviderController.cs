using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.account.provider;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.account.provider;
using Marketplace.Domain.Models.Response.provider;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/account-provider")]
    public class AccountProviderController : DefaultController
    {
        private readonly AccountProviderService _account;
        public AccountProviderController(AccountProviderService accountProviderService)
        {
            _account = accountProviderService;
        }

        [HttpPut, CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> UpdateProvider([FromForm] accountProviderRq _reAcc)
            => await _account.updateProvider(_reAcc);

        [HttpPost]
        public async Task<BaseRs<accountProviderRs>> StoreProvider([FromBody] accountProviderRq _reAcc)
            => await _account.storeProvider(_reAcc);

        #region ..: fetchs :..

        [HttpGet, CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> FindBy()
            => await _account.findByUser();

        [HttpGet("banks"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> GetBanks(string term)
            => await _account.fetchBanks(term);

        [HttpGet("topics"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> GetTopics()
            => await _account.fetchTopics();

        [HttpGet("languages"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> GetLanguages()
            => await _account.fetchLanguages();

        [HttpGet("account-types"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public BaseRs<accountProviderRs> GetAccountTypes()
            => _account.fetchAccountTypes();
        #endregion

        #region ..: schedules :..

        [HttpGet("schedules"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> GetSchedules()
            => await _account.fetchSchedules();

        [HttpPost("schedules"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> SaveSchedules([FromBody] BaseRq<accountProviderRq> _request)
            => await _account.SaveSchedule(_request);

        [HttpDelete("schedules/{id:int}"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<bool>> DeleteSchedule([FromRoute] int id)
            => await _account.DeleteSchedule(id);
        #endregion

        #region ..: appointment :..

        [HttpGet("calendar"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> FetchCalendar(int month = -1)
            => await _account.fetchCalendar(month);

        [HttpGet("appointment/{id}"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> FetchAppointment([FromRoute] int id)
            => await _account.fetchAppointment(id);

        [HttpGet("appointment/{id}/invoice"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> FetchAppointmentInvoice([FromRoute] int id)
            => await _account.fetchAppointmentInvoice(id);

        [HttpGet("appointment/{id}/conference"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<accountProviderRs>> FetchAppointmentConference([FromRoute] int id)
            => await _account.fetchConference(id);
        #endregion

        #region ..: reports :..

        [HttpPost("reports"), CustomAuthorizePermission(Permissions = permission.Account.ViewProvider)]
        public async Task<BaseRs<providerReportsRs>> FetchReports([FromBody] BaseRq<providerReportsRq> Req)
            => await _account.fetchReports(Req);
        #endregion
    }
}