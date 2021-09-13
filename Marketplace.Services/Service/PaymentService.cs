using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Integrations.Payment;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.marketplace;
using Marketplace.Domain.Models.Request.payment;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.marketplace;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class PaymentService
    {   
        private readonly CustomAuthenticatedUser _customerUser;
        private readonly ProviderService _providerService;
        private readonly IConfiguration _configuration;
        private readonly ICustomCache _cache;
        private readonly IPayment _payment;

        public PaymentService(CustomAuthenticatedUser customerUser,
                              ProviderService providerService,
                              IConfiguration configuration,
                              ICustomCache cache,
                              IPayment payment)
        {
            _providerService = providerService;
            _configuration = configuration;
            _customerUser = customerUser;
            _payment = payment;
            _cache = cache;
        }

        public async Task<BaseRs<providerMktRs>> Store(BaseRq<paymentRq> _request)
        {
            var _res = new BaseRs<providerMktRs>();
            try
            {
                var providerRs = (await _providerService.FindById(_request.data.provider_id));
                if(providerRs.error != null)
                {
                    _res.error = providerRs.error;
                    return _res;
                }

                // payment
                var dto = new Domain.Models.dto.payment.PaymentDto()
                {
                    payments = new List<Domain.Models.dto.payment.PaymentList>()
                };

                // provider
                var _paymentDto = new Domain.Models.dto.payment.PaymentList();
                _paymentDto.Provider = providerRs.content.provider[0];

                //productSale = new Domain.Models.dto.payment.ProductSale() { id = provider.id.Value, price = (double)provider.price },
                //    PaymentMethod = _request.data.payment_method,
                //    totalprice = (double)provider.price,
                //    webhook_url = ""


                // add
                dto.payments.Add(_paymentDto);




                // buy
                await _payment.Buy(dto);
            }
            catch (Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}