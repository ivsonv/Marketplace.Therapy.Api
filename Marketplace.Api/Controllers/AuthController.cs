using Marketplace.Domain.Models.Request.auth.customer;
using Marketplace.Domain.Models.Request.auth.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.auth.customer;
using Marketplace.Domain.Models.Response.auth.provider;
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

        [HttpPost("provider")]
        public async Task<BaseRs<providerAuthRs>> ProviderAuth([FromBody] providerAuthRq login)
            => await _authService.Provider(login);

        [HttpPost("provider/reset")]
        public async Task<BaseRs<bool>> ProviderResetPassword([FromBody] providerAuthRq login)
            => await _authService.ProviderResetPassword(login);

        [HttpPut("provider/update-password")]
        public async Task<BaseRs<bool>> ProviderUpdatePassword([FromBody] providerAuthRq _request)
            => await _authService.ProviderUpdatePassword(_request);

        [HttpPost("admin")]
        public async Task<BaseRs<providerAuthRs>> AdminAuth([FromBody] providerAuthRq login)
            => await _authService.Admin(login);

        //[HttpPost("provider/reset")]
        //public async Task<BaseRs<bool>> ProviderResetPassword([FromBody] providerAuthRq login)
        //    => await _authService.ProviderResetPassword(login);

        //[HttpPut("provider/update-password")]
        //public async Task<BaseRs<bool>> ProviderUpdatePassword([FromBody] providerAuthRq _request)
        //    => await _authService.ProviderUpdatePassword(_request);
    }
}