using Marketplace.Domain.Interface.Shared;
namespace Marketplace.Domain.Interface.Marketplace
{
    public interface IUserRepository : ICrudRepository<Entities.User>
    { }
}