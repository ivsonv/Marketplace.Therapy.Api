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
        private readonly IConfiguration _configuration;
        private readonly ICustomCache _cache;
        private readonly IPayment _payment;
        private readonly IMapper _mapper;

        public PaymentService(CustomAuthenticatedUser customerUser,
                              AppointmentService appointmentService,
                              ProviderService providerService,
                              CustomerService customerService,
                              IConfiguration configuration,
                              ICustomCache cache,
                              IPayment payment,
                              IMapper mapper)
        {
            _appointmentService = appointmentService;
            _providerService = providerService;
            _customerService = customerService;
            _configuration = configuration;
            _customerUser = customerUser;
            _payment = payment;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<BaseRs<paymentRs>> Store(BaseRq<paymentRq> _request)
        {
            var _res = new BaseRs<paymentRs>();
            try
            {
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
                var app = new BaseRq<Domain.Models.Request.appointment.appointmentRq>()
                {
                    data = new Domain.Models.Request.appointment.appointmentRq()
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
                //_cache.Clear("calendars");

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

                // Sales
                _paymentDto.productSale = new Domain.Models.dto.payment.ProductSale() { id = app.data.id, price = (double)app.data.price };
                dto.payments.Add(_paymentDto); // add
                await _payment.Buy(dto);  // buy

                // codigo transação.
                app.data.transaction_code = dto.payments[0].transactionCode;

                //logs
                app.data.Logs = new List<Domain.Entities.AppointmentLog>()
                {
                    new Domain.Entities.AppointmentLog()
                    {
                        jsonRq = dto.payments[0].LogRq,
                        jsonRs = dto.payments[0].LogRs,
                        description = "pi"
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
                var appointment = await _appointmentService.FindById(consult.code);
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
                    appointment.content.Customer = null;
                    appointment.content.Provider = null;

                    // atualiza
                    var resUp = await _appointmentService.UpdateStatus(appointment.content);
                    if (resUp.error != null)
                    {
                        // tenta novamente
                        if (resUp.error.message[0].IndexOf("cannot be tracked because another instance with the same key value for {'id'} is already being tracked") > -1)
                            resUp = await _appointmentService.UpdateStatus(appointment.content);

                        if (resUp.error != null)
                            _res.error = resUp.error;
                    }
                }
            }
            catch (Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}