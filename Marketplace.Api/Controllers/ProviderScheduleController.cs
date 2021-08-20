using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.provider;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [CustomAuthorizePermission]
    [Route("api/provider/schedules")]
    public class ProviderScheduleController : DefaultController
    {
        private readonly ProviderScheduleService _providerScheduleService;
        public ProviderScheduleController(ProviderScheduleService providerScheduleService)
        {
            _providerScheduleService = providerScheduleService;
        }

        [HttpGet, CustomAuthorizePermission(Permissions = permission.providerSchedules.View)]
        public async Task<BaseRs<List<providerScheduleRs>>> Show([FromBody] BaseRq<providerScheduleRq> _request)
            => await _providerScheduleService.Show(_request);

        [HttpPost, CustomAuthorizePermission(Permissions = permission.providerSchedules.Create)]
        public async Task<BaseRs<providerScheduleRs>> Store([FromBody] BaseRq<providerScheduleRq> _request)
            => await _providerScheduleService.Store(_request);

        [HttpPut, CustomAuthorizePermission(Permissions = permission.providerSchedules.Edit)]
        public async Task<BaseRs<providerScheduleRs>> Update([FromBody] BaseRq<providerScheduleRq> _request)
            => await _providerScheduleService.Update(_request);

        [HttpDelete("{id:int}"), CustomAuthorizePermission(Permissions = permission.providerSchedules.Delete)]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _providerScheduleService.Delete(id);
    }
}