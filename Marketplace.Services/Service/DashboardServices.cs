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
        private readonly ICustomCache _cache;

        public DashboardService(IAppointmentRepository appointmentRepository,
                                ICustomCache cache)
        {
            _appointmentRepository = appointmentRepository;
            _cache = cache;
        }

        public async Task<BaseRs<dynamic>> fetchReports(BaseRq<Domain.Models.Request.dashboard.AppointmentRq> _request)
        {
            var _res = new BaseRs<dynamic>();
            try
            {
                var lst = await _appointmentRepository.ShowDashboardReports(_request);
                _res.content = lst.ConvertAll(x => new 
                {
                    payment = new {
                        ds = x.payment_status.ToString(),
                        status = x.payment_status,
                    },
                    transaction_code = x.transaction_code,
                    booking_date = x.booking_date.ToString("dd/MM/yyyy HH:mm"),
                    created_at = x.created_at.Value.ToString("dd/MM/yyyy HH:mm"),
                    status = x.status,
                    price = x.price,
                    id = x.id
                });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}