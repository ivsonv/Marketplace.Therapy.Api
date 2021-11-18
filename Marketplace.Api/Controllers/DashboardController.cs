using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.marketplace;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.marketplace;
using Marketplace.Domain.Models.Response.topics;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/dashboard"), CustomAuthorizePermission(Permissions = permission.Dashboard.View)]
    public class DashboardController : DefaultController
    {
        private readonly DashboardService _dashboardService;
        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("overview")]
        public async Task<BaseRs<dynamic>> Overview()
            => await _dashboardService.fetchOverview();

        [HttpGet("reports")]
        public async Task<BaseRs<dynamic>> Show([FromQuery] BaseRq<Domain.Models.Request.dashboard.AppointmentRq> _request)
            => await _dashboardService.fetchReports(_request);

        [HttpGet("appointment/{id}")]
        public async Task<BaseRs<dynamic>> FindbyId([FromRoute] int id)
            => await _dashboardService.fetchAppointmentId(id);

        [HttpGet("appointment/{id}/invoice")]
        public async Task<BaseRs<dynamic>> FetchAppointmentInvoice([FromRoute] int id)
            => await _dashboardService.fetchAppointmentInvoice(id);
    }
}