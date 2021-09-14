using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.payment;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.payment;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/payment")]
    public class PaymentController : DefaultController
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<BaseRs<paymentRs>> Store([FromBody] BaseRq<paymentRq> _request)
            => await _paymentService.Store(_request);
    }
}