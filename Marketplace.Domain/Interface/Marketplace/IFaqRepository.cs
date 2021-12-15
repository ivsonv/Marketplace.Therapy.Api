using Marketplace.Domain.Interface.Shared;
using Marketplace.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface IFaqRepository : ICrudRepository<Entities.Faq>
    {
        Task<List<Entities.Faq>> ShowCache(Pagination pagination, string search = "");
    }

    public interface IFaqQuestionRepository : ICrudRepository<Entities.FaqQuestion>
    { }
}