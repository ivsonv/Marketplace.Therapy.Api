using Marketplace.Domain.Interface.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface IProviderScheduleRepository : ICrudRepository<Entities.ProviderSchedule>
    {
        Task<List<Entities.ProviderSchedule>> Show(int provider_id);
    }
}