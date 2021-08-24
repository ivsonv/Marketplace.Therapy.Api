using Marketplace.Domain.Interface.Shared;
using Marketplace.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface IAppointmentRepository : ICrudRepository<Entities.Appointment>
    {
        Task<List<Entities.Appointment>> Show(Pagination pagination, int provider_id);
    }
}