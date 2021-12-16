using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.Payment;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.payment;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.payment;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Marketplace.Domain.Interface.Integrations.caching;
using AutoMapper;
using Marketplace.Domain.Models.Request.appointment;
using System.Linq;

namespace Marketplace.Services.Service
{
    public class PaymentService
    {
        private readonly CustomAuthenticatedUser _customerUser;
        private readonly AppointmentService _appointmentService;
        private readonly CustomerService _customerService;
        private readonly ProviderService _providerService;
        private readonly EmailService _emailService;

        private readonly IConfiguration _configuration;
        private readonly ICustomCache _cache;
        private readonly IPayment _payment;

        public PaymentService(CustomAuthenticatedUser customerUser,
                              AppointmentService appointmentService,
                              ProviderService providerService,
                              CustomerService customerService,
                              EmailService emailService,
                              IConfiguration configuration,
                              ICustomCache cache,
                              IPayment payment)
        {
            _appointmentService = appointmentService;
            _providerService = providerService;
            _customerService = customerService;
            _configuration = configuration;
            _customerUser = customerUser;
            _emailService = emailService;
            _payment = payment;
            _cache = cache;
        }

        public async Task<BaseRs<paymentRs>> Checkout(BaseRq<paymentRq> _request)
        {
            var _res = new BaseRs<paymentRs>();
            try
            {
                _request.data.holder_cpf = _request.data.holder_cpf.clearMask();
                _request.data.number = _request.data.number.clearMask();

                if (!_request.data.holder_cpf.IsCpf())
                    throw new ArgumentException("CPF do Titular do cartão e inválido");

                if(!_request.data.expire.Contains("/"))
                    throw new ArgumentException("Validade do cartão informada e inválida");

                if (_request.data.expire.Split("/")[1].Length != 4)
                    _request.data.expire.Split("/")[1] = $"20{_request.data.expire.Split("/")[1]}";
                    // throw new ArgumentException("ANO da validade do cartão está incompleto. formato: mm/yyyy");

                    #region ..: records :..

                var providerRs = (await _providerService.FindById(_request.data.provider_id));
                if (providerRs.error != null)
                {
                    _res.error = providerRs.error;
                    return _res;
                }

                var customerRs = await _customerService.FindById(_customerUser.user.id);
                if (customerRs.error != null)
                {
                    _res.error = customerRs.error;
                    return _res;
                }
                #endregion

                // saveAppointment (pending)
                DateTime booking = DateTime.Parse($"{_request.data.date.ToString("yyyy-MM-dd")}T{_request.data.hour}");
                decimal comission = providerRs.content.provider[0].price.PercentValue(20);

                // request
                var app = new BaseRq<appointmentRq>()
                {
                    data = new appointmentRq()
                    {
                        customer_id = customerRs.content.customer[0].id.Value,
                        provider_id = providerRs.content.provider[0].id.Value,
                        booking_date = booking,
                        payment_status = Enumerados.PaymentStatus.pending,
                        status = Enumerados.AppointmentStatus.pending,
                        type = Enumerados.AppointmentType.online_session,
                        origin = Enumerados.AppointmentOrigin.ecommerce,

                        price_transfer = providerRs.content.provider[0].price - comission, // transferir provider
                        price_full = providerRs.content.provider[0].price,                 // preço que o provider cobra
                        price = providerRs.content.provider[0].price,                      // preço que o cliente paga
                        price_commission = comission,                                      // comissão da plataforma
                    }
                };
                await _appointmentService.Store(app);

                // customer
                string _name = providerRs.content.provider[0].nickname.IsNotEmpty()
                    ? providerRs.content.provider[0].nickname
                    : providerRs.content.provider[0].fantasy_name + " " + providerRs.content.provider[0].company_name;

                _emailService.sendAppointment(new Domain.Models.dto.appointment.Email()
                {
                    description = $"Sua consulta com {_name} está PENDENTE no nomento.",
                    name = $"{customerRs.content.customer[0].name}",
                    email = customerRs.content.customer[0].email,
                    title = "Registramos sua consulta.",
                    nick = "Sua consulta está Pendente"
                });

                // payment request
                var dto = new Domain.Models.dto.payment.PaymentDto()
                {
                    payments = new List<Domain.Models.dto.payment.PaymentList>()
                };

                var _paymentDto = new Domain.Models.dto.payment.PaymentList();
                _paymentDto.Customer = customerRs.content.customer[0];
                _paymentDto.Provider = providerRs.content.provider[0];
                _paymentDto.totalprice = (double)_paymentDto.Provider.price;
                _paymentDto.webhook_url = _configuration["payment:webhook"];
                _paymentDto.PaymentMethod = _request.data.payment_method;

                // cartão
                _paymentDto.card = new Domain.Models.dto.payment.CardDto()
                {
                    number = _request.data.number.clearMask(),
                    holder_cpf = _request.data.holder_cpf.clearMask(),
                    holder = _request.data.holder,
                    cvv = _request.data.cvv,
                    expiration_year = _request.data.expire.Split("/")[1],
                    expiration_month = _request.data.expire.Split("/")[0],
                    installment_price = _paymentDto.totalprice,
                    total_price = _paymentDto.totalprice,
                    installment = 1
                };

                // produto
                _paymentDto.productSale = new Domain.Models.dto.payment.ProductSale()
                {
                    price = (double)app.data.price,
                    id = app.data.id,
                    Data = booking
                };
                dto.payments.Add(_paymentDto); // add

                // Pagamento
                await _payment.Buy(dto);

                // codigo transação.
                app.data.authorization_sale = dto.payments[0].authorization_sale;
                app.data.authorization = dto.payments[0].authorization_code;
                app.data.transaction_code = dto.payments[0].transactionCode;
                app.data.payment_status = dto.payments[0].paymentStatus;
                app.data.status = dto.payments[0].status;

                //logs
                app.data.Logs = new List<Domain.Entities.AppointmentLog>()
                {
                    new Domain.Entities.AppointmentLog()
                    {
                        jsonRq = dto.payments[0].LogRq,
                        jsonRs = dto.payments[0].LogRs,
                        description = "checkout"
                    }
                };
                await _appointmentService.Update(app);

                // retorno chamada
                _res.content = new paymentRs()
                {
                    url = dto.payments[0].transactionUrl
                };
            }
            catch (Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<paymentRs>> Consult(consultRq consult)
        {
            var _res = new BaseRs<paymentRs>();
            try
            {
                // appointment
                var appointment = await _appointmentService.FindByIdPayment(consult.code);
                if (appointment.content.transaction_code != consult.token)
                    appointment.content.transaction_code = consult.token;

                // search payment
                var dto = new Domain.Models.dto.payment.PaymentDto()
                {
                    payments = new List<Domain.Models.dto.payment.PaymentList>()
                    {
                        new Domain.Models.dto.payment.PaymentList()
                        {
                            transactionCode = appointment.content.transaction_code,
                            Provider = new Domain.Models.dto.provider.providerDto()
                            {
                                splitAccounts = appointment.content.Provider.SplitAccounts.ToList()
                            }
                        }
                    }
                };
                await _payment.Search(dto);

                // atualizar status
                var _pay = dto.payments[0];
                if (appointment.content.payment_status != _pay.paymentStatus)
                {
                    // Informar paciente
                    string _name = appointment.content.Provider.nickname.IsNotEmpty()
                        ? appointment.content.Provider.nickname
                        : appointment.content.Provider.fantasy_name + " " + appointment.content.Provider.company_name;

                    // logs
                    appointment.content.Logs = new List<Domain.Entities.AppointmentLog>()
                    {
                        new Domain.Entities.AppointmentLog()
                        {
                            description = $"status: {appointment.content.payment_status}>{_pay.paymentStatus}",
                            jsonRq = dto.payments[0].LogRq,
                            jsonRs = dto.payments[0].LogRs
                        }
                    };

                    // atribuir
                    appointment.content.payment_status = _pay.paymentStatus;
                    appointment.content.status = _pay.status;

                    #region ..: EMAILS ALTERAÇÃO STATUS :..

                    if (appointment.content.payment_status == Enumerados.PaymentStatus.confirmed)
                    {
                        // paciente
                        _emailService.sendAppointment(new Domain.Models.dto.appointment.Email()
                        {
                            description = $"Sua consulta com {_name} está {appointment.content.payment_status.dsPayment()}. <br>" +
                           $"Data: {appointment.content.booking_date.ToString("dd/MM/yyyy")} <br>" +
                           $"Hora: {appointment.content.booking_date.ToString("HH:mm")}h <br> " +
                           $"Fuso Horário de SÃO PAULO",

                            nick = $"Sua consulta está {appointment.content.payment_status.dsPayment()}",
                            name = $"{appointment.content.Customer.name}",
                            email = appointment.content.Customer.email,
                            title = "Sua consulta está confirmada."
                        });

                        // Informar psico
                        _emailService.sendAppointment(new Domain.Models.dto.appointment.Email()
                        {
                            description = $"{appointment.content.Customer.name} agendou uma consulta com você. <br><br>" +
                            $"Data: {appointment.content.booking_date.ToString("dd/MM/yyyy")} <br>" +
                            $"Hora: {appointment.content.booking_date.ToString("HH:mm")}h <br> " +
                            $"Fuso Horário de SÃO PAULO",

                            nick = $"Voce tem uma consulta Agendada {appointment.content.payment_status.dsPayment()}",
                            name = $"{appointment.content.Provider.fantasy_name}",
                            email = appointment.content.Provider.email,
                            title = "Nova consulta está confirmada"
                        });
                    }
                    #endregion

                    //
                    appointment.content.Customer = null;
                    appointment.content.Provider = null;

                    // atualiza
                    var resUp = await _appointmentService.UpdateStatus(appointment.content);
                    _cache.Clear();
                }
            }
            catch (Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<paymentRs>> Cancel(cancelRq cancel)
        {
            var _res = new BaseRs<paymentRs>();
            try
            {
                // appointment
                var appointment = await _appointmentService.FindById(cancel.code);
                if (CustomExtensions.DateNow > appointment.content.booking_date)
                    _res.setError("Data de agendamento já passou, não e possível cancelar.");

                if (appointment.content.payment_status != Enumerados.PaymentStatus.confirmed)
                    _res.setError("Apenas pedidos confirmados, que podem ser cancelados.");

                if (_res.error == null)
                {
                    // search payment
                    var dto = new Domain.Models.dto.payment.PaymentDto()
                    {
                        payments = new List<Domain.Models.dto.payment.PaymentList>()
                        {
                            new Domain.Models.dto.payment.PaymentList()
                            {
                                transactionCode = appointment.content.transaction_code,
                                totalprice = (double)appointment.content.price,
                                Provider = new Domain.Models.dto.provider.providerDto()
                                {
                                    splitAccounts = appointment.content.Provider.SplitAccounts.ToList()
                                }
                            }
                        }
                    };
                    await _payment.Cancel(dto);

                    // atualizar status
                    var _pay = dto.payments[0];
                    if (_pay.cancel)
                    {
                        // logs
                        appointment.content.Logs = new List<Domain.Entities.AppointmentLog>()
                        {
                            new Domain.Entities.AppointmentLog()
                            {
                                description = cancel.pacient ? "cancelado por cliente" : "cancelado por psi",
                                jsonRq = dto.payments[0].LogRq,
                                jsonRs = dto.payments[0].LogRs
                            }
                        };

                        // atribuir
                        appointment.content.payment_status = Enumerados.PaymentStatus.canceled;
                        appointment.content.status = Enumerados.AppointmentStatus.canceled;

                        try
                        {
                            // e-mail cancelamento.
                            if (!cancel.pacient)
                            {
                                // psi teve cancelar.
                                // paciente
                                _emailService.sendAppointment(new Domain.Models.dto.appointment.Email()
                                {
                                    description = $"Sua consulta com {appointment.content.Provider.fantasy_name} está cancelada. <br><br>" +
                                    $"Infelizmente o psicólogo teve um problema e precisou cancelar, já solicitamos o estorno do seu pagamento.<br>" +
                                    $"Data: {appointment.content.booking_date.ToString("dd/MM/yyyy")} <br>" +
                                    $"Hora: {appointment.content.booking_date.ToString("HH:mm")}h <br> " +
                                    $"Fuso Horário de SÃO PAULO",

                                    nick = $"Sua consulta está cancelada",
                                    name = $"{appointment.content.Customer.name}",
                                    email = appointment.content.Customer.email,
                                    title = "Sua consulta está cancelada."
                                });

                                // Informar psico
                                _emailService.sendAppointment(new Domain.Models.dto.appointment.Email()
                                {
                                    description = $"você cancelou uma consulta com {appointment.content.Customer.name}. <br><br>" +
                                    $"Dados do agendamento: <br>" +
                                    $"Data: {appointment.content.booking_date.ToString("dd/MM/yyyy")} <br>" +
                                    $"Hora: {appointment.content.booking_date.ToString("HH:mm")}h <br> " +
                                    $"Fuso Horário de SÃO PAULO",

                                    nick = $"consulta cancelada",
                                    name = $"{appointment.content.Provider.fantasy_name}",
                                    email = appointment.content.Provider.email,
                                    title = $"consulta está cancelada #{appointment.content.id}"
                                });
                            }
                            else
                            {   // paciente cancelou
                                _emailService.sendAppointment(new Domain.Models.dto.appointment.Email()
                                {
                                    description = $"Sua consulta com {appointment.content.Provider.fantasy_name} está cancelada. <br><br>" +
                                    $"Já solicitamos o estorno do seu pagamento.<br>" +
                                    $"Dados do seu agendamento: <br>" +
                                    $"Data: {appointment.content.booking_date.ToString("dd/MM/yyyy")} <br>" +
                                    $"Hora: {appointment.content.booking_date.ToString("HH:mm")}h <br> " +
                                    $"Fuso Horário de SÃO PAULO",

                                    nick = $"Sua consulta está cancelada",
                                    name = $"{appointment.content.Customer.name}",
                                    email = appointment.content.Customer.email,
                                    title = "Sua consulta está cancelada."
                                });

                                // Informar psico
                                _emailService.sendAppointment(new Domain.Models.dto.appointment.Email()
                                {
                                    description = $"{appointment.content.Customer.name} cancelou uma consulta com você. <br><br>" +
                                    $"Data: {appointment.content.booking_date.ToString("dd/MM/yyyy")} <br>" +
                                    $"Hora: {appointment.content.booking_date.ToString("HH:mm")}h <br> " +
                                    $"Fuso Horário de SÃO PAULO",

                                    nick = $"consulta cancelada",
                                    name = $"{appointment.content.Provider.fantasy_name}",
                                    email = appointment.content.Provider.email,
                                    title = $"consulta está cancelada #{appointment.content.id}"
                                });
                            }
                        }
                        catch { }

                        // customer
                        appointment.content.Customer = null;
                        appointment.content.Provider = null;

                        // atualiza
                        var resUp = await _appointmentService.UpdateStatus(appointment.content);

                    }
                }
            }
            catch (Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}