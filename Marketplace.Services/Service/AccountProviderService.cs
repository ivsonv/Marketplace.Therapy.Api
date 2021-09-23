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
using Marketplace.Domain.Models.Response.appointment;
using Marketplace.Domain.Models.Response.provider;
using Marketplace.Domain.Models.Response.users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class AccountProviderService
    {
        private readonly CustomAuthenticatedUser _authenticatedProvider;
        private readonly ProviderScheduleService _scheduleService;
        private readonly AppointmentService _appointmentService;
        private readonly ProviderService _providerService;
        private readonly UploadService _uploadService;
        private readonly BankService _bankService;
        private readonly ICustomCache _cache;
        private readonly IMapper _mapper;

        public AccountProviderService(ProviderScheduleService scheduleService,
                                      AppointmentService appointmentService,
                                      ProviderService providerService,
                                      CustomAuthenticatedUser user,
                                      UploadService uploadService,
                                      BankService bankService,
                                      ICustomCache cache,
                                      IMapper mapper)
        {
            _appointmentService = appointmentService;
            _providerService = providerService;
            _scheduleService = scheduleService;
            _uploadService = uploadService;
            _authenticatedProvider = user;
            _bankService = bankService;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<BaseRs<accountProviderRs>> storeProvider(accountProviderRq _request)
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                // request provider
                var _rq = new BaseRq<providerRq>()
                {
                    data = new providerRq()
                    {
                        company_name = _request.company_name,
                        fantasy_name = _request.fantasy_name,
                        password = _request.password,
                        email = _request.email
                    }
                };

                // retorno provider
                var _resUpdate = await _providerService.Store(_rq);
                if (_resUpdate.error == null)
                    _res.content = new accountProviderRs() { provider = _resUpdate.content };
                else
                    _res.error = _resUpdate.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<accountProviderRs>> updateProvider(accountProviderRq _request)
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                // convert
                var entity = _request.data.Deserialize<providerRq>();

                // valid
                if (entity.id != _authenticatedProvider.user.id)
                    return new BaseRs<accountProviderRs>() { error = new BaseError(new List<string>() { "Solicitação inválida." }) };

                #region ..: imagens :..

                // imagem
                if (!_request.profile.IsEmpty())
                {
                    // remover imagem atual S3
                    if (entity.image.IsNotEmpty())
                        await _uploadService.RemoveImage(entity.image, "profile");

                    entity.image = string.Format("{0}.{1}", CustomExtensions.getGuid, _request.profile.FileName.getExtension());
                    await _uploadService.UploadImage(_request.profile, entity.image, "profile");
                }

                // assinatura
                if (!_request.sig.IsEmpty())
                {
                    // receipts
                    if (entity.receipts.IsEmpty())
                        return new BaseRs<accountProviderRs>() { error = new BaseError(new List<string>() { "Assinatura - Solicitação inválida." }) };

                    // remover imagem atual S3, caso tenha
                    if (entity.receipts[0].signature.IsNotEmpty())
                        await _uploadService.RemoveImage(entity.receipts[0].signature, "signature");

                    // key
                    entity.receipts[0].signature = string.Format("{0}.{1}", CustomExtensions.getGuid, _request.sig.FileName.getExtension());

                    // signature
                    await _uploadService.UploadImage(_request.sig, entity.receipts[0].signature, "signature");
                }
                #endregion

                // request provider
                var _rq = new BaseRq<providerRq>()
                {
                    data = entity
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
                _request.data.schedule.provider_id = _authenticatedProvider.user.id;

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
                if (entity.content.provider_id != _authenticatedProvider.user.id)
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
                _res.content.provider = (await _providerService.FindById(_authenticatedProvider.user.id)).content;
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
        public BaseRs<accountProviderRs> fetchAccountTypes()
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                _res.content = new accountProviderRs()
                {
                    accounttypes = _bankService.getAccountTypes()
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
                        provider_id = _authenticatedProvider.user.id
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

        // calendar
        public async Task<BaseRs<accountProviderRs>> fetchCalendar(int month)
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                // -1 = mes atual
                month = month == -1 ? CustomExtensions.DateNow.Month : month;

                // list
                var appointmets = await _cache.GetCalendar();

                // cache tras mes atual
                if (CustomExtensions.DateNow.Month != month)
                {
                    // outro mês
                    if (!appointmets.Any())
                        appointmets = await _cache.GetCalendar(month);
                }

                // appointments
                _res.content = new accountProviderRs();
                _res.content.appointments = appointmets
                                             .Where(w => w.provider_id == _authenticatedProvider.user.id)
                                             .Where(w => w.booking_date.Month == month)
                                             .Select(s => new providerAppointment()
                                             {
                                                 customer = new customerAppointment()
                                                 {
                                                     name = s.Customer.name,
                                                     id = s.customer_id
                                                 },
                                                 startds = s.booking_date.ToString("yyyy-MM-dd"),
                                                 hour = s.booking_date.TimeOfDay,
                                                 type = s.type,
                                                 id = s.id
                                             }).ToList();
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountProviderRs>> fetchAppointment(int id)
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                var resApp = await _appointmentService.FindByAppointment(appointment_id: id);
                if (resApp.error == null && resApp.content != null)
                {
                    // apenas agendamento do provedor.
                    if (resApp.content.Provider.id == _authenticatedProvider.user.id)
                        _res.content = new accountProviderRs()
                        {
                            appointment = resApp.content
                        };
                }
                else
                    _res.error = resApp.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountProviderRs>> fetchAppointmentInvoice(int id)
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                var resApp = await _appointmentService.FindByAppointmentInvoice(appointment_id: id);
                if (resApp.error == null && resApp.content != null)
                {
                    // apenas agendamento do cliente.
                    if (resApp.content.Provider.id == _authenticatedProvider.user.id)
                    {
                        _res.content = new accountProviderRs()
                        {
                            appointment = resApp.content
                        };
                    }
                }
                else _res.error = resApp.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}