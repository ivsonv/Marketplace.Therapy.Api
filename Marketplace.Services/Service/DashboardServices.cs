using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.topics;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.topics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class DashboardService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public DashboardService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<BaseRs<dynamic>> fetchReports(BaseRq<Domain.Models.Request.dashboard.AppointmentRq> _request)
        {
            var _res = new BaseRs<dynamic>();
            try
            {
                _res.content = (await _appointmentRepository.ShowDashboardReports(_request))
                    .ConvertAll(x => new
                    {
                        payment = new
                        {
                            ds = x.payment_status.ToString(),
                            status = x.payment_status,
                        },
                        booking_date = x.booking_date.ToString("dd/MM/yyyy HH:mm"),
                        created_at = x.created_at.Value.ToString("dd/MM/yyyy HH:mm"),
                        transaction_code = x.transaction_code,
                        dsstatus = x.status.ToString(),
                        status = x.status,
                        price = $"R$ {x.price.ToString("N2")}",
                        id = x.id
                    });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<dynamic>> fetchAppointmentId(int id)
        {
            var _res = new BaseRs<dynamic>();
            try
            {
                var _apt = await _appointmentRepository.FindByAppointmentDashboard(id);
                if (_apt != null)
                    _res.content = new
                    {
                        customer = new
                        {
                            email = _apt.Customer.email,
                            name = _apt.Customer.name,
                            id = _apt.Customer.id,
                        },
                        provider = new
                        {
                            name = $"{_apt.Provider.fantasy_name} {_apt.Provider.company_name}",
                            email = _apt.Provider.email,
                            id = _apt.Provider.id,
                        },
                        payment = new
                        {
                            transaction_code = _apt.transaction_code,
                            ds = _apt.payment_status.ToString(),
                            status = _apt.payment_status,
                        },
                        booking_date = _apt.booking_date.ToString("dd/MM/yyyy HH:mm"),
                        created_at = _apt.created_at.Value.ToString("dd/MM/yyyy HH:mm"),
                        status = _apt.status,
                        price = _apt.price,
                        id = _apt.id
                    };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}