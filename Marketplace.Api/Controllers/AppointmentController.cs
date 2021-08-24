using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.banks;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.banks;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers
{
    [Route("api/appointment")]
    public class AppointmentController : DefaultController
    {
        private readonly AppointmentService _appointmentService;
        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        //[HttpGet, CustomAuthorizePermission(Permissions = permission.Bank.View)]
        //public async Task<BaseRs<List<bankRs>>> Show([FromQuery] BaseRq<bankRq> _request)
        //    => await _appointmentService.show(_request);

        //[HttpGet("{id:int}"), CustomAuthorizePermission(Permissions = permission.Bank.View)]
        //public async Task<BaseRs<bankRs>> FindById([FromRoute] int id)
        //    => await _appointmentService.FindById(id);

        //[HttpPost, CustomAuthorizePermission(Permissions = permission.Bank.Create)]
        //public async Task<BaseRs<bankRs>> Store([FromBody] BaseRq<bankRq> _request)
        //    => await _appointmentService.Store(_request);

        //[HttpPut, CustomAuthorizePermission(Permissions = permission.Bank.Edit)]
        //public async Task<BaseRs<bankRs>> Update([FromBody] BaseRq<bankRq> _request)
        //    => await _appointmentService.Update(_request);

        //[HttpDelete("{id:int}"), CustomAuthorizePermission(Permissions = permission.Bank.Delete)]
        //public async Task<BaseRs<bool>> Delete([FromRoute] int id)
        //    => await _appointmentService.Delete(id);
    }
}