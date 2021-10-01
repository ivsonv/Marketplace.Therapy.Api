using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
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

        [HttpPost, CustomAuthorizePermission(Permissions = permission.Account.ViewCustomer)]
        public async Task<BaseRs<paymentRs>> Checkout([FromBody] BaseRq<paymentRq> _request)
            => await _paymentService.Checkout(_request);

        [HttpPost("consult")]
        public async Task<BaseRs<paymentRs>> Consult([FromBody] consultRq Req)
            => await _paymentService.Consult(Req);

        [HttpPost("cancel"), CustomAuthorizePermission(Permissions = permission.Account.ViewCustomer + "," + permission.Account.ViewProvider)]
        public async Task<BaseRs<paymentRs>> Cancel([FromBody] cancelRq Req)
            => await _paymentService.Cancel(Req);
    }
}