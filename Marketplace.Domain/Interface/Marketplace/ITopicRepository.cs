using Marketplace.Domain.Interface.Shared;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface ITopicRepository : ICrudRepository<Entities.Topic>
    {

    }
}