using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Integrations.caching
{
    public interface ICustomCache
    {
        void Clear();
        Task<List<Entities.Bank>> GetBanks();
        Task<List<Entities.Topic>> GetTopics();
        Task<List<Entities.Provider>> GetProviders();
        Task<List<Entities.Language>> GetLanguages();
        List<Entities.GroupPermission> GetPermissions();
        Task<List<Entities.Appointment>> GetCalendar();
        Task<List<Entities.Appointment>> GetCalendar(int mes);
    }
}