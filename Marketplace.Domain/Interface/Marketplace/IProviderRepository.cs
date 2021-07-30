using Marketplace.Domain.Interface.Shared;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface IProviderRepository : ICrudRepository<Entities.Provider>
    {
        Task<Entities.Provider> FindByEmail(string email);
        Task<Entities.Provider> FindByCnpj(string cnpj);
        Task<Entities.Provider> FindAuthByEmail(string email);
    }
}