using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.appointment;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.appointment;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<BaseRs<appointmentRs>> Store([FromBody] BaseRq<appointmentRq> _request)
            => await _appointmentService.Store(_request);

        //[HttpPut, CustomAuthorizePermission(Permissions = permission.Bank.Edit)]
        //public async Task<BaseRs<bankRs>> Update([FromBody] BaseRq<bankRq> _request)
        //    => await _appointmentService.Update(_request);

        //[HttpDelete("{id:int}"), CustomAuthorizePermission(Permissions = permission.Bank.Delete)]
        //public async Task<BaseRs<bool>> Delete([FromRoute] int id)
        //    => await _appointmentService.Delete(id);
    }
}