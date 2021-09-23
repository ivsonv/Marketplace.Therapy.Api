using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.appointment;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.account.customer;
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

        [HttpPost]
        public async Task<BaseRs<appointmentRs>> Store([FromBody] BaseRq<appointmentRq> _request)
            => await _appointmentService.Store(_request);
    }
}