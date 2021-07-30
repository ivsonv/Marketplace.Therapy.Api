using Microsoft.AspNetCore.Http;

namespace Marketplace.Domain.Helpers
{
    public class CustomAuthenticatedUser
    {
        private readonly IHttpContextAccessor _accessor;
        public CustomAuthenticatedUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        //public AuthDTO ActiveUser
        //{
        //    get
        //    {
        //        return _accessor.HttpContext.User.Claims
        //                         .FirstOrDefault(f => f.Type == System.Security.Claims.ClaimTypes.UserData)?
        //                         .Value.Deserialize<AuthDTO>();
        //    }
        //}

    }
}