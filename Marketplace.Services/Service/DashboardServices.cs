using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.topics;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.topics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class DashboardService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly AppointmentService _appointmentService;
        public DashboardService(IAppointmentRepository appointmentRepository,
                                AppointmentService appointmentService)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
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
                            name = _apt.Customer.name,
                            id = _apt.Customer.id,
                        },
                        provider = new
                        {
                            name = $"{_apt.Provider.fantasy_name} {_apt.Provider.company_name}",
                            id = _apt.Provider.id,
                        },
                        payment = new
                        {
                            transaction_code = _apt.transaction_code,
                            status = _apt.payment_status.ToString(),
                            type = _apt.type.ToString()
                        },
                        logs = !_apt.Logs.IsEmpty()
                        ? _apt.Logs.OrderByDescending(o => o.created_at)
                                   .Select(s => new
                                   {
                                       created_at = s.created_at.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                                       description = s.description,
                                   })
                        : null
                        ,
                        booking_date = _apt.booking_date.ToString("dd/MM/yyyy HH:mm"),
                        created_at = _apt.created_at.Value.ToString("dd/MM/yyyy HH:mm"),
                        status = _apt.status.ToString(),
                        price = "R$ " + _apt.price.ToString("N2"),
                        id = _apt.id
                    };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<dynamic>> fetchAppointmentInvoice(int id)
        {
            var _res = new BaseRs<dynamic>();
            try
            {
                var resApp = await _appointmentService.FindByAppointmentInvoice(appointment_id: id);
                if (resApp.error == null && resApp.content != null)
                    _res.content = resApp.content;
                else
                    _res.error = resApp.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}