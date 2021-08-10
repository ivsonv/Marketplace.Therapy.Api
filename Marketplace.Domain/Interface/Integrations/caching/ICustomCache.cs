using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Integrations.caching
{
    public interface ICustomCache
    {
        Task<List<Entities.Bank>> GetBanks();
    }
}
