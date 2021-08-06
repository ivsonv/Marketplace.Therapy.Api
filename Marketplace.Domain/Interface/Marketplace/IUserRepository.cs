using Marketplace.Domain.Interface.Shared;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface IUserRepository : ICrudRepository<Entities.User>
    {
        Task<Entities.User> FindByEmail(string email);
        Task<Entities.User> FindAuthByEmail(string email);
    }
}