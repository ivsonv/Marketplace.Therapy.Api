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
    [Route("api/account")]
    public class AccountController : DefaultController
    {
        private readonly ProviderScheduleService _providerScheduleService;
        private readonly ProviderService _providerService;
        public AccountController(ProviderScheduleService providerScheduleService,
                                 ProviderService providerService)
        {
            _providerScheduleService = providerScheduleService;
            _providerService = providerService;
        }

        #region ..: Providers :..

        [HttpPost("provider")]
        public async Task<BaseRs<providerRs>> Store([FromBody] BaseRq<providerRq> _request)
          => await _providerService.Store(_request);

        [HttpPut("provider")]
        public async Task<BaseRs<providerRs>> Update([FromBody] BaseRq<providerRq> _request)
            => await _providerService.Update(_request);

        [HttpGet("provider/situations")]
        public dynamic ShowSituations()
            => _providerService.getSituations();

        [HttpGet("provider/{id:int}")]
        public async Task<BaseRs<providerRs>> FindById([FromRoute] int id)
            => await _providerService.FindById(id);
        #endregion

        #region ..: Providers Schedule :..

        [HttpGet("provider/schedules")]
        public async Task<BaseRs<List<providerScheduleRs>>> SchedulesShow([FromBody] BaseRq<providerScheduleRq> _request)
            => await _providerScheduleService.Show(_request);

        [HttpPost("provider/schedules")]
        public async Task<BaseRs<providerScheduleRs>> ProviderSchedules([FromBody] BaseRq<providerScheduleRq> _request)
            => await _providerScheduleService.Store(_request);
        #endregion

        #region ..: customer :..

        #endregion
    }
}