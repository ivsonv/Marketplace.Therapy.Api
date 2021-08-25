using Microsoft.AspNetCore.Http;

namespace Marketplace.Domain.Models.Request.account.provider
{
    public class accountProviderRq : dto.provider.providerDto
    {
        public string data { get; set; }
        public IFormFile profile { get; set; }
        public IFormFile signature { get; set; }

        public Request.provider.providerScheduleRq schedule { get; set; }
    }
}