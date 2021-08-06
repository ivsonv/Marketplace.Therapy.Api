using Marketplace.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Shared
{
    public interface ICrudRepository<T>
    {
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<T> FindById(int id);
        Task<List<T>> Show(Pagination pagination, string seach = "");
        Task<List<T>> Show(Pagination pagination);
    }
}
