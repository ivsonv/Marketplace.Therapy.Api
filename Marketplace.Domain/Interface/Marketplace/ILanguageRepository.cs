using Marketplace.Domain.Interface.Shared;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface ILanguageRepository : ICrudRepository<Entities.Language>
    { }
}