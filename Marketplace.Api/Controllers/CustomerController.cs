using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.customers;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.customers;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/customers")]
    public class CustomerController : DefaultController
    {
        private readonly CustomerService _customerService;
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<BaseRs<customerRs>> Show([FromQuery] BaseRq<customerRq> _request)
            => await _customerService.show(_request);

        [HttpGet("{id:int}")]
        public async Task<BaseRs<customerRs>> FindById([FromRoute] int id)
            => await _customerService.FindById(id);

        [HttpPost]
        public async Task<BaseRs<customerRs>> Store([FromBody] BaseRq<customerRq> _request)
            => await _customerService.Store(_request);

        [HttpPut]
        public async Task<BaseRs<customerRs>> Update([FromBody] BaseRq<customerRq> _request)
            => await _customerService.Update(_request);

        [HttpDelete("{id:int}")]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _customerService.Delete(id);
    }
}