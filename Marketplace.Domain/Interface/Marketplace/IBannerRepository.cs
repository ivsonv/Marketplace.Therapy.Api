using Marketplace.Domain.Interface.Shared;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface IBannerRepository : ICrudRepository<Entities.Banner>
    { }
}