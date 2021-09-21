using Marketplace.Domain.Interface.Shared;
using Marketplace.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface IAppointmentRepository : ICrudRepository<Entities.Appointment>
    {
        Task<List<Entities.Appointment>> Show(Pagination pagination, int provider_id);
        Task<List<Entities.Appointment>> ShowByCustomer(Pagination pagination, int customer_id);
        Task<Entities.Appointment> FindByAppointmentIdCustomer(int customer_id, int appointment_id);
        Task<Entities.Appointment> FindByAppointmentInvoice(int customer_id, int appointment_id);
    }
}