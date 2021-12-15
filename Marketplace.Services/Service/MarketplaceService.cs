using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Integrations.Payment;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.marketplace;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.marketplace;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class MarketplaceService
    {
        private readonly ProviderScheduleService _scheduleService;
        private readonly FaqService _faqService;
        private readonly IConfiguration _configuration;
        private readonly ICustomCache _cache;

        public MarketplaceService(ProviderScheduleService scheduleService,
                                  IConfiguration configuration,
                                  FaqService faqService,
                                  ICustomCache cache)
        {
            _scheduleService = scheduleService;
            _configuration = configuration;
            _faqService = faqService;
            _cache = cache;
        }

        public async Task<BaseRs<List<providerMktRs>>> ShowProviders(BaseRq<providerMktRq> _request)
        {
            var _res = new BaseRs<List<providerMktRs>>();
            if (_request.data == null)
                _request.data = new providerMktRq();
            try
            {
                // active    = psico que controlar
                // completed = cadastro completo
                var list = (await _cache.GetProviders()).Where(w => w.completed && w.active && w.image.IsNotEmpty());

                if (_request.data.name.IsNotEmpty())
                    list = list.Where(w => w.fantasy_name.IsCompare().Contains(_request.data.name.IsCompare()) ||
                                           w.company_name.IsCompare().Contains(_request.data.name.IsCompare()) ||
                                           w.nickname.IsCompare().Contains(_request.data.name.IsCompare())
                                           ).ToList();

                // list
                _request.pagination.size = 10; //force
                _res.content = list
                    .OrderBy(o => o.fantasy_name)
                    .Skip(_request.pagination.size * _request.pagination.page)
                    .Take(_request.pagination.size)
                    .Select(x => new providerMktRs()
                    {
                        name = x.nickname.IsNotEmpty() ? x.nickname : $"{x.fantasy_name} {x.company_name}",
                        image = x.image.toImageUrl($"{_configuration["storage:image"]}/profile"),
                        state = !x.Address.IsEmpty()
                        ? x.Address.First().uf : null,
                        introduction = x.description,
                        price = x.price,
                        link = x.link,
                        crp = x.crp
                    }).ToList();
            }
            catch (Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<providerMktRs>> FindByProvider(string link)
        {
            var _res = new BaseRs<providerMktRs>();
            try
            {
                var provider = (await _cache.GetProviders()).FirstOrDefault(f => f.link.IsCompare() == link.IsCompare());
                _res.content = new providerMktRs()
                {
                    name = provider.nickname.IsNotEmpty() ? provider.nickname : $"{provider.fantasy_name} {provider.company_name}",
                    image = provider.image.toImageUrl($"{_configuration["storage:image"]}/profile"),
                    state = !provider.Address.IsEmpty()
                        ? provider.Address.First().uf : null,
                    introduction = provider.description,
                    biography = provider.biography,
                    price = provider.price,
                    link = provider.link,
                    crp = provider.crp,
                    id = provider.id,
                };

                // topics
                if (!provider.Topics.IsEmpty())
                {
                    // topics do provider
                    var ids = provider.Topics.Select(s => s.topic_id);

                    // pegar topics vinculado
                    var topics = (await _cache.GetTopics()).Where(w => w.active && ids.Any(a => a == w.id)).ToList();

                    // separar experiencia / especialidade
                    if (topics.Any())
                    {
                        // experiencia
                        if (topics.Any(a => a.experience))
                            _res.content.experiences = topics.Where(w => w.experience)
                                                             .Select(s => new providerMktItem()
                                                             {
                                                                 name = s.name
                                                             });

                        // especialidade
                        if (topics.Any(a => !a.experience))
                            _res.content.expertises = topics.Where(w => !w.experience)
                                                         .Select(s => new providerMktItem()
                                                         {
                                                             name = s.name
                                                         });
                    }
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<providerMktRs>> AvailableHours(string link, DateTime dtStart)
        {
            var _res = new BaseRs<providerMktRs>() { content = new providerMktRs() };
            try
            {
                // provider
                var provider = (await _cache.GetProviders()).FirstOrDefault(f => f.link.IsCompare() == link.IsCompare());

                // schedule
                var schedule = await _scheduleService.Show(new BaseRq<Domain.Models.Request.provider.providerScheduleRq>()
                {
                    data = new Domain.Models.Request.provider.providerScheduleRq()
                    {
                        provider_id = provider.id
                    }
                });

                // horarios
                if (schedule.content.IsNotEmpty())
                {
                    if (dtStart == DateTime.MinValue)
                        dtStart = CustomExtensions.DateNow;
                    else
                    {
                        dtStart.AddHours(CustomExtensions.DateNow.Hour);
                    }

                    // buscar agendamentos ativos do psicologo
                    var appoints = (await _cache.GetAppointmentsActive()).Where(w => w.provider_id == provider.id).ToList();

                    // qtd dias
                    for (int i = 0; i < 4; i++)
                    {
                        var pp = new providerMktDate();
                        pp.ds_date = $"{dtStart.AddDays(i).Date.Day.ToString("00")}/{dtStart.AddDays(i).Date.Month.toMonthds().Substring(0, 3).ToUpper()}";
                        pp.ds_week = ((int)dtStart.AddDays(i).Date.DayOfWeek).toWeekds().Split(' ')[0].ToUpper();
                        pp.date = dtStart.AddDays(i);

                        // 24 horas
                        pp.hours = new List<providerMktDateHour>();
                        for (int h = 0; h < 24; h++)
                        {
                            // Data de hoje
                            // não mostrar horarios que já passou.
                            if (i == 0 && h < (pp.date.TimeOfDay.Hours + 1))
                                continue;

                            // atende naquele semana
                            bool _disponivel = schedule.content.Any(w => w.day_week == (int)pp.date.DayOfWeek);
                            if (_disponivel)
                            {
                                // configuração da semana
                                var week = schedule.content.First(w => w.day_week == (int)pp.date.DayOfWeek);

                                // 1 primeiro horário
                                var _hour = new providerMktDateHour()
                                {
                                    hour = TimeSpan.Parse($"{h.ToString("00")}:00:00")
                                };

                                // dentro da faixa configurada pelo provider ?
                                if (_hour.hour >= week.start &&
                                    _hour.hour <= week.end)
                                {
                                    // verifica se já tem horário agendando
                                    var dt = DateTime.Parse($"{pp.date.ToString("yyyy-MM-dd")}T{_hour.hour}");
                                    var dtEnd = dt.AddMinutes(50); // 50 minutos
                                    if (this.isHourOpen(appoints, dt, dtEnd))
                                        pp.hours.Add(_hour);
                                }

                                // 2 segundo horário
                                var _hour2 = new providerMktDateHour()
                                {
                                    hour = TimeSpan.Parse($"{h.ToString("00")}:30:00")
                                };

                                // dentro da faixa configurada pelo provider ?
                                if (_hour2.hour >= week.start &&
                                    _hour2.hour <= week.end)
                                {
                                    // verifica se já tem horário agendando
                                    var dt = DateTime.Parse($"{pp.date.ToString("yyyy-MM-dd")}T{_hour2.hour}");
                                    var dtEnd = dt.AddMinutes(50); // 50 minutos
                                    if (this.isHourOpen(appoints, dt, dtEnd))
                                        pp.hours.Add(_hour2);
                                }
                            }
                        }

                        // popular disponibilidade.
                        if (_res.content.dates == null)
                            _res.content.dates = new List<providerMktDate>();

                        // add
                        _res.content.dates.Add(pp);
                    }
                    _res.content.displayDates = _res.content.dates.Max(m => m.hours.Count);
                }
            }
            catch (Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<List<Domain.Entities.Faq>>> ShowFaq()
        {
            var _res = new BaseRs<List<Domain.Entities.Faq>>();
            try
            {
                _res.content = await _cache.GetFaq();
            }
            catch (Exception ex) { _res.setError(ex); }
            return _res;
        }

        private bool isHourOpen(List<Domain.Entities.Appointment> appoints, DateTime dtStart, DateTime dtEnd)
        {
            return !appoints.Where(w => (w.booking_date >= dtStart && w.booking_date <= dtEnd)
                                     || ((w.booking_date.AddMinutes(50)) >= dtStart && (w.booking_date.AddMinutes(50)) <= dtEnd)).Any();

            //return appoints.Where(ap => ((ap.booking_date < dtStart && ap.booking_date > dtStart)
            //                          || (ap.booking_date < dtEnd && ap.booking_date > dtEnd))
            //                          || (dtStart <= ap.booking_date && dtEnd >= ap.booking_date && dtStart != ap.booking_date)).Any();
        }
    }
}