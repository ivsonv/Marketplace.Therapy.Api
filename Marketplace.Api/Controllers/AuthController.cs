using Marketplace.Domain.Models.Request.auth.customer;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.auth.customer;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : DefaultController
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("customer")]
        public async Task<BaseRs<customerAuthRs>> CustomerAuth([FromBody] customerAuthRq login)
            => await _authService.Customer(login);

        [HttpPost("customer/reset")]
        public async Task<BaseRs<bool>> CustomerResetPassword([FromBody] customerAuthRq login)
            => await _authService.CustomerResetPassword(login);

        [HttpPut("customer/update-password")]
        public async Task<BaseRs<bool>> CustomerUpdatePassword([FromBody] customerAuthRq _request)
            => await _authService.CustomerUpdatePassword(_request);
    }
}