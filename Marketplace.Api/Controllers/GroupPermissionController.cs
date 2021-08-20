using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.groupPermissions;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.groupPermissions;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [CustomAuthorizePermission]
    [Route("api/group-permission")]
    public class GroupPermissionController : DefaultController
    {
        private readonly GroupPermissionService _groupPermissionService;
        public GroupPermissionController(GroupPermissionService groupPermissionService)
        {
            _groupPermissionService = groupPermissionService;
        }

        [HttpGet]
        public async Task<BaseRs<List<groupPermissionRs>>> Show([FromQuery] BaseRq<groupPermissionRq> _request)
            => await _groupPermissionService.show(_request);

        [HttpGet("{id:int}")]
        public async Task<BaseRs<groupPermissionRs>> FindById([FromRoute] int id)
            => await _groupPermissionService.FindById(id);

        [HttpPost]
        public async Task<BaseRs<groupPermissionRs>> Store([FromBody] BaseRq<groupPermissionRq> _request)
            => await _groupPermissionService.Store(_request);

        [HttpPut]
        public async Task<BaseRs<groupPermissionRs>> Update([FromBody] BaseRq<groupPermissionRq> _request)
            => await _groupPermissionService.Update(_request);

        [HttpDelete("{id:int}")]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _groupPermissionService.Delete(id);

        [HttpGet("permissions")]
        public dynamic ShowRoles() => Domain.Models.permissions.permission.GetPermissions();
    }
}