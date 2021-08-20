using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.dto.auth;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Marketplace.Api.Controllers
{
    public class DefaultController : ControllerBase
    {
        protected AuthDto MyUser
        {
            get
            {
                return base.User.Claims
                                 .FirstOrDefault(f => f.Type == System.Security.Claims.ClaimTypes.UserData)?
                                 .Value.Deserialize<AuthDto>();
            }
        }
    }
}