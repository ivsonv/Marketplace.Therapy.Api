using Marketplace.Domain.Interface.Shared;
using Marketplace.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface ITopicRepository : ICrudRepository<Entities.Topic>
    {
        Task<List<Entities.Topic>> ShowCache(Pagination pagination, string seach = "");
    }
}