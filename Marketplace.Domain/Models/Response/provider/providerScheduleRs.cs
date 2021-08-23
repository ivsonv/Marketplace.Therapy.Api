using Marketplace.Domain.Helpers;

namespace Marketplace.Domain.Models.Response.provider
{
    public class providerScheduleRs : Entities.ProviderSchedule
    {
        public string ds_week
        {
            get { return $"{base.day_week.toWeekds()}"; }
        }
    }
}
