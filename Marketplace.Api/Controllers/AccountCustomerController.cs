using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.account.customer;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.account.customer;
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

        [HttpGet, CustomAuthorizePermission(Permissions = permission.Account.ViewCustomer)]
        public async Task<BaseRs<accountCustomerRs>> FindBy()
            => await _account.findByUser();

        [HttpPost]
        public async Task<BaseRs<accountCustomerRs>> StoreCustomer([FromBody] accountCustomerRq _reAcc)
            => await _account.storeCustomer(_reAcc);

        [HttpPut, CustomAuthorizePermission(Permissions = permission.Account.ViewCustomer)]
        public async Task<BaseRs<accountCustomerRs>> UpdateProvider([FromBody] accountCustomerRq _reAcc)
            => await _account.updateCustomer(_reAcc);

        [HttpGet("appointments"), CustomAuthorizePermission(Permissions = permission.Account.ViewCustomer)]
        public async Task<BaseRs<accountCustomerRs>> FetchCalendar(BaseRq<string> _rec)
            => await _account.fetchAppointments(_rec);

        [HttpGet("appointment/{id}"), CustomAuthorizePermission(Permissions = permission.Account.ViewCustomer)]
        public async Task<BaseRs<accountCustomerRs>> FetchAppointment([FromRoute] int id)
            => await _account.fetchAppointment(id);

        [HttpGet("appointment/{id}/invoice"), CustomAuthorizePermission(Permissions = permission.Account.ViewCustomer)]
        public async Task<BaseRs<accountCustomerRs>> FetchAppointmentInvoice([FromRoute] int id)
            => await _account.fetchAppointmentInvoice(id);

        [HttpGet("appointment/{id}/conference"), CustomAuthorizePermission(Permissions = permission.Account.ViewCustomer)]
        public async Task<BaseRs<accountCustomerRs>> FetchAppointmentConference([FromRoute] int id)
            => await _account.fetchConference(id);

        [HttpGet("appointment/{id}/conference/finish"), CustomAuthorizePermission(Permissions = permission.Account.ViewCustomer)]
        public async Task<BaseRs<accountCustomerRs>> FinishConference([FromRoute] int id)
            => await _account.finishConference(id);


        [HttpPut("appointment/reeschedule")]
        public async Task<BaseRs<dynamic>> ReescheduleAppointment([FromBody] BaseRq<Domain.Models.Request.dashboard.AppointmentRq> _request)
            => await _account.ReecheduleAppointment(_request);
    }
}