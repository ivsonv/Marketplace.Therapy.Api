using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Models.dto.auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

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

                // Ler permissões do usuario
                //var _permissionList = _cache.GetAuthorizes()
                //                                 .Where(w => user.Permissions.Any(g => g == w.id))
                //                                 .SelectMany(s => s.Permissions)
                //                                 .Select(s => s.name)
                //                                 .Distinct().ToList();

                //// verificar se tem a permissão.
                //if (_permissionList.Any(a => a == Permissions))
                //    return; // tem pode continuar
            }

            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
