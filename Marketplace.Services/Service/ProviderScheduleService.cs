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
                if (_request.data.provider_id <= 0)
                    _res.setError("provider_id não informado.");

                if (_res.error == null)
                    _res.content = (await _providerScheduleRepository.Show(_request.data.provider_id))
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

        public async Task<BaseRs<providerScheduleRs>> FindById(int id)
        {
            var _res = new BaseRs<providerScheduleRs>();
            try
            {
                var entity = await _providerScheduleRepository.FindById(id);
                if (entity != null)
                {
                    _res.content = new providerScheduleRs();
                    _res.content.provider_id = entity.provider_id;
                    _res.content.day_week = entity.day_week;
                    _res.content.start = entity.start;
                    _res.content.end = entity.end;
                    _res.content.id = entity.id;
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<bool>> Delete(int id)
        {
            var _res = new BaseRs<bool>();
            try
            {
                await _providerScheduleRepository.Delete(await _providerScheduleRepository.FindById(id));
                _res.content = true;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<bool>> Delete(Domain.Entities.ProviderSchedule schedule)
        {
            var _res = new BaseRs<bool>();
            try
            {
                await _providerScheduleRepository.Delete(schedule);
                _res.content = true;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}