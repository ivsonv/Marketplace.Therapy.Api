using AutoMapper;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.account.provider;
using Marketplace.Domain.Models.Request.appointment;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Request.users;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.account.provider;
using Marketplace.Domain.Models.Response.users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class AccountProviderService
    {
        private readonly CustomAuthenticatedUser _authenticatedUser;
        private readonly ProviderScheduleService _scheduleService;
        private readonly AppointmentService _appointmentService;
        private readonly ProviderService _providerService;
        private readonly ICustomCache _cache;
        private readonly IMapper _mapper;

        public AccountProviderService(ProviderScheduleService scheduleService,
                                      AppointmentService appointmentService,
                                      ProviderService providerService,
                                      CustomAuthenticatedUser user,
                                      ICustomCache cache,
                                      IMapper mapper)
        {
            _appointmentService = appointmentService;
            _providerService = providerService;
            _scheduleService = scheduleService;
            _authenticatedUser = user;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<BaseRs<accountProviderRs>> updateProvider(BaseRq<accountProviderRq> _request)
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                if (_request.data.id != _authenticatedUser.user.id)
                    return new BaseRs<accountProviderRs>() { error = new BaseError(new List<string>() { "Solicitação inválida." }) };

                // request provider
                var _rq = new BaseRq<providerRq>()
                {
                    data = _mapper.Map<providerRq>(_request.data)
                };

                // retorno provider
                var _resUpdate = await _providerService.Update(_rq);
                if (_resUpdate.error == null)
                    _res.content = new accountProviderRs() { provider = _resUpdate.content };
                else
                    _res.error = _resUpdate.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountProviderRs>> SaveSchedule(BaseRq<accountProviderRq> _request)
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                // setando usuário
                _request.data.schedule.provider_id = _authenticatedUser.user.id;

                // request
                var _rq = new BaseRq<providerScheduleRq>()
                {
                    data = _mapper.Map<providerScheduleRq>(_request.data.schedule)
                };

                // atualizar
                var _reSchedule = _rq.data.id > 0
                    ? await _scheduleService.Update(_rq)
                    : await _scheduleService.Store(_rq);

                // retorno
                if (_reSchedule.error == null)
                    _res.content = new accountProviderRs()
                    {
                        schedules = new List<Domain.Models.Response.provider.providerScheduleRs>()
                        {
                            _reSchedule.content
                        }
                    };
                else
                    _res.error = _reSchedule.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<bool>> DeleteSchedule(int schedule_id)
        {
            var _res = new BaseRs<bool>();
            try
            {
                var entity = await _scheduleService.FindById(schedule_id);
                if (entity.content == null)
                    return new BaseRs<bool>() { error = new BaseError(new List<string>() { "Solicitação inválida." }) };

                // excluido pertence ao user.
                if (entity.content.provider_id != _authenticatedUser.user.id)
                    return new BaseRs<bool>() { error = new BaseError(new List<string>() { "Solicitação inválida." }) };

                // remover
                var _reSchedule = await _scheduleService.Delete(entity.content);

                // retorno
                if (_reSchedule.error == null)
                    _reSchedule.content = true;
                else
                    _res.error = _reSchedule.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<accountProviderRs>> findByUser()
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                _res.content = new accountProviderRs();
                _res.content.provider = (await _providerService.FindById(_authenticatedUser.user.id)).content;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountProviderRs>> fetchLanguages()
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                _res.content = new accountProviderRs()
                {
                    languages = await _cache.GetLanguages()
                };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountProviderRs>> fetchTopics()
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                _res.content = new accountProviderRs()
                {
                    topics = await _cache.GetTopics()
                };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountProviderRs>> fetchBanks(string _term)
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                _res.content = new accountProviderRs()
                {
                    banks = (await _cache.GetBanks()).Where(w => _term.IsEmpty() ||
                                                            w.name.IsCompare().Contains(_term.IsCompare()) ||
                                                            w.code.IsCompare().Contains(_term.IsCompare()))
                                                     .Where(w => w.active)
                                                     .Select(s => new Domain.Models.Response.banks.bankRs()
                                                     {
                                                         code = s.code,
                                                         name = s.name
                                                     }).ToList()
                };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountProviderRs>> fetchSchedules()
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                var fetch = new BaseRq<providerScheduleRq>()
                {
                    data = new providerScheduleRq()
                    {
                        provider_id = _authenticatedUser.user.id
                    }
                };

                _res.content = new accountProviderRs()
                {
                    schedules = (await _scheduleService.Show(fetch)).content.OrderBy(o => o.day_week).ToList()
                };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<accountProviderRs>> fetchAppointment(BaseRq<accountProviderRq> _req)
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                // -1 = mes atual
                //month = month == -1 ? CustomExtensions.DateNow.Month : month;

                // fetch
                var fetch = new BaseRq<appointmentRq>()
                {
                    pagination = _req.pagination,
                    data = new appointmentRq()
                    {
                        provider_id = _authenticatedUser.user.id,
                        //month = month
                    }
                };

                _res.content = new accountProviderRs()
                {
                    appointments = (await _appointmentService.showByProvider(fetch)).content
                };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}