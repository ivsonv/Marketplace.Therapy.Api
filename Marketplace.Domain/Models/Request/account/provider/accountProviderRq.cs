namespace Marketplace.Domain.Models.Request.account.provider
{
    public class accountProviderRq : dto.provider.providerDto
    {
        public Request.provider.providerScheduleRq schedule { get; set; }
    }
}
