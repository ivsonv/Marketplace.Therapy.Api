﻿using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.account.customer;
using Marketplace.Domain.Models.Request.customers;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.account.customer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class AccountCustomerService
    {
        private readonly CustomAuthenticatedUser _authenticatedCustomer;
        private readonly AppointmentService _appointmentService;
        private readonly CustomerService _customerService;
        private readonly EmailService _emailService;
        private readonly DashboardService _dashboardService;

        public AccountCustomerService(AppointmentService appointmentService,
                                      CustomerService customerService,
                                      EmailService emailService,
                                      DashboardService dashboardService,
                                      CustomAuthenticatedUser user)
        {
            _appointmentService = appointmentService;
            _dashboardService = dashboardService;
            _customerService = customerService;
            _authenticatedCustomer = user;
            _emailService = emailService;
        }

        public async Task<BaseRs<accountCustomerRs>> storeCustomer(accountCustomerRq _request)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                _request.cpf = _request.cpf.clearMask();
                if (!_request.cpf.IsCpf())
                {
                    _res.setError("CPF Informado não e válido.");
                    return _res;
                }

                var _rq = new BaseRq<customerRq>()
                {
                    data = new customerRq()
                    {
                        name = _request.name.Clear().ToUpper(),
                        password = _request.password,
                        email = _request.email,
                        cpf = _request.cpf
                    }
                };

                // retorno customer
                var _resUpdate = await _customerService.Store(_rq);
                if (_resUpdate.error == null)
                    _res.content = new accountCustomerRs() { customer = _resUpdate.content };
                else
                    _res.error = _resUpdate.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountCustomerRs>> updateCustomer(accountCustomerRq _request)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                if (_request.cpf.IsEmpty())
                {
                    _res.setError("CPF e obrigatório.");
                    return _res;
                }

                _request.cpf = _request.cpf.clearMask();
                if (!_request.cpf.IsCpf())
                {
                    _res.setError("CPF Informado não e válido.");
                    return _res;
                }

                var _rq = new BaseRq<customerRq>()
                {
                    data = new customerRq()
                    {
                        name = _request.name.Clear().ToUpper(),
                        id = _authenticatedCustomer.user.id,
                        email = _request.email,
                        cpf = _request.cpf
                    }
                };

                // retorno customer
                var _resUpdate = await _customerService.Update(_rq);
                if (_resUpdate.error == null)
                    _res.content = new accountCustomerRs() { customer = _resUpdate.content };
                else
                    _res.error = _resUpdate.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountCustomerRs>> fetchAppointments(BaseRq<string> _request)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                var rq = new BaseRq<Domain.Models.Request.appointment.appointmentRq>()
                {
                    pagination = _request.pagination,
                    data = new Domain.Models.Request.appointment.appointmentRq()
                    {
                        customer_id = _authenticatedCustomer.user.id
                    }
                };

                var lst = await _appointmentService.showByCustomer(rq);
                if (lst.content.IsNotEmpty())
                {
                    _res.content = new accountCustomerRs();
                    _res.content.appointments = lst.content.ConvertAll(cc => new CustomerAppointment()
                    {
                        provider_name = $"{cc.Provider.fantasy_name} {cc.Provider.company_name}",
                        data = $"{cc.booking_date.ToString("dd/MM/yyyy")}",
                        hora = $"{cc.booking_date.ToString("HH:mm")}",
                        dsStatus = cc.status.dsStatus(),
                        id = cc.id
                    });
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountCustomerRs>> fetchAppointment(int id)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                var resApp = await _appointmentService.FindByAppointment(appointment_id: id);
                if (resApp.error == null && resApp.content != null)
                {
                    // apenas agendamento do cliente.
                    if (resApp.content.Customer.id == _authenticatedCustomer.user.id)
                    {
                        _res.content = new accountCustomerRs()
                        {
                            appointment = resApp.content,
                        };

                        _res.content.appointment.room_name = $"{_res.content.appointment.Provider.fantasy_name} {_res.content.appointment.Provider.company_name}";
                        _res.content.appointment.room_id = $"clique-terapia-{_res.content.appointment.id.ToString("000000")}";
                        _res.content.appointment.transaction_code = _res.content.appointment.transaction_code;
                        _res.content.appointment.dsStatus = _res.content.appointment.status.dsStatus();
                    }
                }
                else
                    _res.error = resApp.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountCustomerRs>> fetchAppointmentInvoice(int id)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                var resApp = await _appointmentService.FindByAppointmentInvoice(appointment_id: id);
                if (resApp.error == null && resApp.content != null)
                {
                    // apenas agendamento do cliente.
                    if (resApp.content.Customer.id == _authenticatedCustomer.user.id)
                    {
                        _res.content = new accountCustomerRs()
                        {
                            appointment = resApp.content
                        };
                    }
                }
                else
                    _res.error = resApp.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountCustomerRs>> findByUser()
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                _res.content = new accountCustomerRs();
                _res.content.customer = (await _customerService.FindById(_authenticatedCustomer.user.id)).content;

                if (_res.content.customer != null && !_res.content.customer.customer.IsEmpty())
                    _res.content.customer.customer.ForEach(fe => { fe.password = null; });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<accountCustomerRs>> fetchConference(int id)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                var resApp = await _appointmentService.FindByAppointmentConferenceInit(appointment_id: id);
                if (resApp.error == null && resApp.content != null)
                {
                    // apenas agendamento do cliente.
                    if (resApp.content.Customer.id == _authenticatedCustomer.user.id)
                    {
                        // retornar
                        _res.content = new accountCustomerRs()
                        {
                            appointment = resApp.content
                        };

                        // registrar log
                        await _appointmentService.RegistrarLog(id, "pct entrou na sala.");
                    }
                    else
                        _res.error = new BaseError(new List<string>() { "Agendamento não encontrado." });
                }
                else
                    _res.error = resApp.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountCustomerRs>> finishConference(int id)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                var resApp = await _appointmentService.FindByAppointmentConferenceFinish(appointment_id: id);
                if (resApp.error == null && resApp.content != null)
                {
                    // apenas agendamento do cliente.
                    if (resApp.content.Customer.id == _authenticatedCustomer.user.id)
                    {
                        // registrar log
                        await _appointmentService.RegistrarLog(id, "pct saiu da sala.");

                        string msg = "Obrigado por confiar na clique terapia, <br> estamos muito feliz de você ter dado esse grande passo.";
                        _emailService.sendDefault(resApp.content.Customer.email, "Sua Sessão foi encerrada", resApp.content.Customer.name, msg);
                    }
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<dynamic>> ReecheduleAppointment(BaseRq<Domain.Models.Request.dashboard.AppointmentRq> _request)
        {
            _request.data.customer_id = _authenticatedCustomer.user.id;
            return await _dashboardService.UpdateAppointment(_request);
        }
    }
}