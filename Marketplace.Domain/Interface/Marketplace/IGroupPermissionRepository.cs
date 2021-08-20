using Marketplace.Domain.Interface.Shared;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface IGroupPermissionRepository : ICrudRepository<Entities.GroupPermission>
    {
        Task<Entities.GroupPermission> FindByName(string name);
    }
}