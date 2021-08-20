using Marketplace.Domain.Models.dto.auth;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Marketplace.Domain.Helpers
{
    public class CustomAuthenticatedUser
    {
        private readonly IHttpContextAccessor _accessor;
        public CustomAuthenticatedUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public AuthDto user
        {
            get
            {
                return _accessor.HttpContext.User.Claims
                                 .FirstOrDefault(f => f.Type == System.Security.Claims.ClaimTypes.UserData)?
                                 .Value.Deserialize<AuthDto>();
            }
        }

    }
}