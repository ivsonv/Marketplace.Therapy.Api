using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.users;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.users;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [CustomAuthorizePermission]
    [Route("api/user")]
    public class UserController : DefaultController
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<BaseRs<List<userRs>>> Show([FromQuery] BaseRq<userRq> _request)
            => await _userService.show(_request);

        [HttpGet("{id:int}")]
        public async Task<BaseRs<userRs>> FindById([FromRoute] int id)
            => await _userService.FindById(id);

        [HttpPost]
        public async Task<BaseRs<userRs>> Store([FromBody] BaseRq<userRq> _request)
            => await _userService.Store(_request);

        [HttpPut]
        public async Task<BaseRs<userRs>> Update([FromBody] BaseRq<userRq> _request)
            => await _userService.Update(_request);

        [HttpDelete("{id:int}")]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _userService.Delete(id);
    }
}