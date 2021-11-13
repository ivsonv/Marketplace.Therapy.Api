using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.customers;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.customers;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [CustomAuthorizePermission]
    [Route("api/customers")]
    public class CustomerController : DefaultController
    {
        private readonly CustomerService _customerService;
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet, CustomAuthorizePermission(Permissions = permission.Customer.View)]
        public async Task<BaseRs<customerRs>> Show([FromQuery] BaseRq<customerRq> _request)
            => await _customerService.show(_request);

        [HttpGet("{id:int}"), CustomAuthorizePermission(Permissions = permission.Customer.View)]
        public async Task<BaseRs<customerRs>> FindById([FromRoute] int id)
            => await _customerService.FindById(id);

        [HttpGet("{id:int}/appointments"), CustomAuthorizePermission(Permissions = permission.Customer.View)]
        public async Task<dynamic> ShowAppointments([FromRoute] int id)
            => await _customerService.ShowAppointments(id);

        [HttpPost, CustomAuthorizePermission(Permissions = permission.Customer.Create)]
        public async Task<BaseRs<customerRs>> Store([FromBody] BaseRq<customerRq> _request)
            => await _customerService.Store(_request);

        [HttpPut, CustomAuthorizePermission(Permissions = permission.Customer.Edit)]
        public async Task<BaseRs<customerRs>> Update([FromBody] BaseRq<customerRq> _request)
            => await _customerService.Update(_request);

        [HttpDelete("{id:int}"), CustomAuthorizePermission(Permissions = permission.Customer.Delete)]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _customerService.Delete(id);
    }
}