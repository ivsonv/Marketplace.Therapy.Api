using Marketplace.Domain.Interface.Shared;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface ICategoryRepository : ICrudRepository<Entities.Category>
    { }
}