using AutoMapper;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.appointment;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.appointment;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository,
                                  IConfiguration configuration,
                                  IMapper mapper)
        {
            _repository = appointmentRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<BaseRs<appointmentRs>> Store(BaseRq<appointmentRq> _request)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                await _repository.Create(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<appointmentRs>> Update(BaseRq<appointmentRq> _request)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                await _repository.Update(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<appointmentRs>> UpdateStatus(Appointment _app)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                await _repository.Update(_app);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<List<appointmentRs>>> show(BaseRq<appointmentRq> _request)
        {
            var _res = new BaseRs<List<appointmentRs>>();
            try
            {
                var lst = await _repository.Show(_request.pagination, _request.search);
                _res.content = lst.ConvertAll(cc => new appointmentRs() { id = cc.id });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<List<appointmentRs>>> showByProvider(BaseRq<appointmentRq> _request)
        {
            var _res = new BaseRs<List<appointmentRs>>();
            try
            {
                var lst = await _repository.Show(_request.pagination, _request.data.provider_id);
                _res.content = lst.ConvertAll(cc => new appointmentRs()
                {
                    Customer = new Customer() { name = cc.Customer.name },
                    price_transfer = cc.price_transfer,
                    booking_date = cc.booking_date,
                    type = cc.type,
                    id = cc.id
                });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<List<appointmentRs>>> showByCustomer(BaseRq<appointmentRq> _request)
        {
            var _res = new BaseRs<List<appointmentRs>>();
            try
            {
                var lst = await _repository.ShowByCustomer(_request.pagination, _request.data.customer_id);
                if (lst.Any())
                {
                    _res.content = new List<appointmentRs>();
                    lst.ForEach(fe =>
                    {
                        if (fe.status == Enumerados.AppointmentStatus.pending)
                        {
                            if (fe.created_at.Value.ToString("dd/MM/yyyy") == CustomExtensions.DateNow.ToString("dd/MM/yyyy"))
                                if (!lst.Any(a => a.status == Enumerados.AppointmentStatus.confirmed && a.created_at.Value.ToString("dd/MM/yyyy") == CustomExtensions.DateNow.ToString("dd/MM/yyyy")))
                                    _res.content.Add(_mapper.Map<appointmentRs>(fe));
                        }
                        else
                            _res.content.Add(_mapper.Map<appointmentRs>(fe));
                    });
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<appointmentRs>> FindById(int appointment_id)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                var app = await _repository.FindById(appointment_id);
                _res.content = _mapper.Map<appointmentRs>(app);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<appointmentRs>> FindByIdPayment(int appointment_id)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                var app = await _repository.FindByPayment(appointment_id);
                _res.content = _mapper.Map<appointmentRs>(app);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<appointmentRs>> FindByAppointment(int appointment_id)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                var app = await _repository.FindByAppointmentDetails(appointment_id: appointment_id);
                if (app != null)
                {
                    _res.content = _mapper.Map<appointmentRs>(app);
                    _res.content.dsStatusPayment = _res.content.payment_status.ToString();
                    _res.content.start = _res.content.booking_date.ToString("dd/MM/yyyy");
                    _res.content.transaction_code = _res.content.transaction_code;
                    _res.content.hour = _res.content.booking_date.TimeOfDay;
                    _res.content.dsStatus = _res.content.status.dsStatus();
                    _res.content.id = appointment_id;
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<appointmentRs>> FindByAppointmentInvoice(int appointment_id)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                var app = await _repository.FindByAppointmentInvoice(appointment_id: appointment_id);
                if (app != null)
                {
                    if (app.payment_status != Enumerados.PaymentStatus.confirmed)
                        _res.setError("Não e possível gerar Recibo, Agendamento cancelado.");
                    else
                    {
                        _res.content = _mapper.Map<appointmentRs>(app);
                        _res.content.dsStatusPayment = _res.content.payment_status.ToString();
                        _res.content.dsStatus = _res.content.status.ToString();
                        _res.content.start = _res.content.booking_date.ToString("dd/MM/yyyy");
                        _res.content.hour = _res.content.booking_date.TimeOfDay;
                        _res.content.issued = Domain.Helpers.CustomExtensions.DateNow.ToString("dd/MM/yyyy");
                        _res.content.Provider.password = null;

                        if (_res.content.Provider.Receipts.Any())
                            _res.content.Provider.Receipts.First().signature = $"{_configuration["storage:image"]}/signature/{_res.content.Provider.Receipts.First().signature}";
                        else
                            _res.setError("psicólogo não tem assinatura cadastrada, acione suporte.");
                    }
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<appointmentRs>> FindByAppointmentConferenceInit(int appointment_id)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                var app = await _repository.FindByAppointmentConference(appointment_id: appointment_id);
                if (app != null)
                {
                    #region ..: validations :..

                    if (CustomExtensions.DateNow.Date != app.booking_date.Date)
                    {
                        _res.error = new BaseError(new List<string>() { "sessão fora do período." });
                        return _res;
                    }

                    // antes do horário previsto
                    var start = app.booking_date.AddMinutes(-5);
                    if (CustomExtensions.DateNow.TimeOfDay < start.TimeOfDay)
                    {
                        _res.error = new BaseError(new List<string>() { "Só e permitido iniciar 5 minutos antes do horário agendado." });
                        return _res;
                    }

                    // expirou
                    var dtEnd = app.booking_date.AddMinutes(60);
                    if (CustomExtensions.DateNow.TimeOfDay > dtEnd.TimeOfDay)
                    {
                        _res.error = new BaseError(new List<string>() { "Agendamento expirou." });
                        return _res;
                    }
                    #endregion

                    var timeDiff = CustomExtensions.DateNow.TimeOfDay - app.booking_date.TimeOfDay;
                    if (timeDiff.TotalMinutes > 60)
                    {
                        _res.error = new BaseError(new List<string>() { "Agendamento expirou." });
                        return _res;
                    }

                    _res.content = new appointmentRs()
                    {
                        Provider = new Provider() { id = app.provider_id },
                        Customer = new Customer() { id = app.customer_id },
                        room_name = $"{app.Provider.fantasy_name} {app.Provider.company_name}",
                        room_id = $"clique-terapia-{appointment_id.ToString("000000")}",
                        room_traveled = (int)timeDiff.TotalMinutes < 0 ? 0 : (int)timeDiff.TotalMinutes
                    };
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<appointmentRs>> FindByAppointmentConferenceFinish(int appointment_id)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                var app = await _repository.FindByAppointmentConference(appointment_id: appointment_id);
                _res.content = new appointmentRs()
                {
                    Customer = app.Customer,
                    Provider = app.Provider
                };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task RegistrarLog(int appointment_id, string msg)
            => await _repository.RegisterLog(appointment_id, msg);
    }
}