using Marketplace.Domain.Interface.Shared;
using Marketplace.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface ILanguageRepository : ICrudRepository<Entities.Language>
    {
        Task<List<Entities.Language>> ShowCache(Pagination pagination, string seach = "");
    }
}