using AutoMapper;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.dto.provider;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.provider;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class ProviderScheduleService
    {
        private readonly IProviderScheduleRepository _providerScheduleRepository;

        public ProviderScheduleService(IProviderScheduleRepository providerScheduleRepository)
        {
            _providerScheduleRepository = providerScheduleRepository;
        }

        public async Task<BaseRs<List<providerScheduleRs>>> Show(BaseRq<providerScheduleRq> _request)
        {
            var _res = new BaseRs<List<providerScheduleRs>>();
            try
            {
                _res.content = (await _providerScheduleRepository.Show(_request.pagination))
                               .Select(s => new providerScheduleRs()
                               {
                                   day_week = s.day_week,
                                   start = s.start,
                                   end = s.end,
                                   id = s.id
                               }).ToList();
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<providerScheduleRs>> Store(BaseRq<providerScheduleRq> _request)
        {
            var _res = new BaseRs<providerScheduleRs>();
            try
            {
                await _providerScheduleRepository.Create(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<providerScheduleRs>> Update(BaseRq<providerScheduleRq> _request)
        {
            var _res = new BaseRs<providerScheduleRs>();
            try
            {
                await _providerScheduleRepository.Update(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}