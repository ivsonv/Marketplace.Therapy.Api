using Marketplace.Domain.Interface.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface ICustomerRepository : ICrudRepository<Entities.Customer>
    {
        Task<Entities.Customer> FindByEmail(string email);
        Task<Entities.Customer> FindByCpf(string cpf);
        Task<Entities.Customer> FindByToken(string token);
        Task<Entities.Customer> FindAuthByEmail(string email);
        Task<List<Entities.Appointment>> ShowAppointments(int customer_id);
    }
}