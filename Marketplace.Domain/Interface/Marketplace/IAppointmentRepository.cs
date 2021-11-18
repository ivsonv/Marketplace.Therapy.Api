using Marketplace.Domain.Interface.Shared;
using Marketplace.Domain.Models;
using Marketplace.Domain.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Marketplace
{
    public interface IAppointmentRepository : ICrudRepository<Entities.Appointment>
    {
        Task<List<Entities.Appointment>> Reports(Pagination pagination, string term, DateTime dtStart, DateTime dtEnd, int provider_id);
        Task<List<Entities.Appointment>> ShowByCustomer(Pagination pagination, int customer_id);
        Task<List<Entities.Appointment>> Show(Pagination pagination, int provider_id);
        Task<Entities.Appointment> FindByAppointmentDetails(int appointment_id);
        Task<Entities.Appointment> FindByAppointmentInvoice(int appointment_id);
        Task<Entities.Appointment> FindByAppointmentConference(int appointment_id);
        Task<List<Entities.Appointment>> ShowDashboardReports(BaseRq<Models.Request.dashboard.AppointmentRq> rq);
        Task<Entities.Appointment> FindByAppointmentDashboard(int id);
        Task<Entities.Appointment> FindByPayment(int appointment_id);
        Task RegisterLog(int appointment_id, string description);

        /// <summary>
        /// ultimos 3 meses resumo
        /// </summary>
        /// <returns></returns>
        Task<List<Entities.Appointment>> ShowOverview();
    }
}