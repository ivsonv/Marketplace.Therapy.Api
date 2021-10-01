using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Models.dto.auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Linq;

namespace Marketplace.Domain.Helpers
{
    public class CustomAuthorizePermission : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Permissions { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (this.Permissions.IsEmpty())
                return;

            var user = context.HttpContext.User.Claims
                 .FirstOrDefault(f => f.Type == ClaimTypes.UserData)?
                 .Value.Deserialize<AuthDto>();

            if (user != null)
            {
                // não requisitar no banco
                var _cache = (ICustomCache)context.HttpContext.RequestServices.GetService(typeof(ICustomCache));

                // permissions
                var _permissions = _cache.GetPermissions()
                                         .Where(w => user.permissions.Any(g => g == w.id))
                                         .SelectMany(s => s.PermissionsAttached)
                                         .Select(s => s.name)
                                         .Distinct().ToList();

                // verificar se tem a permissão.
                if (_permissions.Any(a => Permissions.Split(',').Any(aa => aa == a)))
                    return; // tem pode continuar
            }

            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
