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
        private readonly ICustomerRepository _customerRepository;
        private readonly IProviderRepository _providerRepository;

        private readonly AppointmentService _appointmentService;
        private readonly EmailService _emailService;
        public DashboardService(IAppointmentRepository appointmentRepository,
                                ICustomerRepository customerRepository,
                                IProviderRepository providerRepository,
                                AppointmentService appointmentService,
                                EmailService emailService)
        {
            _providerRepository = providerRepository;
            _customerRepository = customerRepository;
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
            _emailService = emailService;
        }

        public async Task<BaseRs<dynamic>> fetchOverviewPsiAndCustomer()
        {
            return new BaseRs<dynamic>()
            {
                content = new
                {
                    customer = new { qtd = await _customerRepository.getQtdCliente() },
                    provider = new { qtd = await _providerRepository.getQtdProvider() },
                }
            };
        }
        public async Task<BaseRs<dynamic>> fetchOverview()
        {
            var lst = await _appointmentRepository.ShowOverview();

            var _ret = new List<Domain.Models.dto.dashboard.overview>();
            foreach (var item in lst.GroupBy(o => o.created_at.Value.Month))
            {
                _ret.Add(new Domain.Models.dto.dashboard.overview()
                {
                    mes = item.First().created_at.Value.ToString("MMM/yy").ToUpper(),
                    price_sales = item.Where(w => w.payment_status == Enumerados.PaymentStatus.confirmed).Sum(s => s.price).ToString("N2"),
                    price_sales_revenue = item.Where(w => w.payment_status == Enumerados.PaymentStatus.confirmed).Sum(s => s.price_commission).ToString("N2"),
                    total_appointment_canceled = item.Where(w => w.payment_status == Enumerados.PaymentStatus.canceled).Count().ToString(),
                    total_appointment = item.Where(w => w.payment_status == Enumerados.PaymentStatus.confirmed).Count().ToString(),
                });
            }
            return new BaseRs<dynamic>() { content = _ret };

            // total de cliente ultimo mes
            // total de psi ativos e inativos
            // total faturamento
            // total cancelamento
            // total comissão
            // ultimos 3 meses
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
                        start = _apt.booking_date.ToString("yyyy-MM-dd"),
                        time = _apt.booking_date.ToString("HH:mm:ss"),
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

        public async Task<BaseRs<dynamic>> UpdateAppointment(BaseRq<Domain.Models.Request.dashboard.AppointmentRq> _request)
        {
            var _res = new BaseRs<dynamic>();
            try
            {
                if (!_request.data.start.HasValue || _request.data.start <= CustomExtensions.DateNow)
                    return new BaseRs<dynamic>() { error = new BaseError("Data Informada não e válida.") };

                var entity = await _appointmentRepository.FindById(_request.data.id);

                if (_request.data.customer_id > 0)
                    if (_request.data.customer_id != entity.customer_id)
                    {
                        return new BaseRs<dynamic>() { error = new BaseError("Agendamento não existe para você.") };
                    }

                entity.booking_date = _request.data.start.Value;

                await _appointmentRepository.Update(entity);
                await _appointmentRepository.RegisterLog(entity.id, $"Remarcação, nova data {entity.booking_date.ToString("dd/MM/yyyy")}");

                // disparar email
                try
                {
                    string name = entity.Provider.nickname.IsNotEmpty()
                     ? entity.Provider.nickname
                     : entity.Provider.fantasy_name + " " + entity.Provider.company_name;

                    _emailService.sendAppointment(new Domain.Models.dto.appointment.Email()
                    {
                        description = $"Sua consulta com {name} está {entity.payment_status.dsPayment()}. <br>" +
                        $"Data: {entity.booking_date.ToString("dd/MM/yyyy")} <br>" +
                        $"Hora: {entity.booking_date.ToString("HH:mm")}h <br> " +
                        $"Fuso Horário de SÃO PAULO",

                        nick = $"Consulta Remarcada está {entity.payment_status.dsPayment()} #{entity.id.ToString("000000")}",
                        name = $"{entity.Customer.name}",
                        email = entity.Customer.email,
                        title = $"reagendamento de consulta confirmada #{entity.id.ToString("000000")}"
                    });
                    await _appointmentRepository.RegisterLog(entity.id, $"Remarcação, Email enviado ao pct.");
                }
                catch { }

                // Informar psico
                try
                {
                    _emailService.sendAppointment(new Domain.Models.dto.appointment.Email()
                    {
                        description = $"{entity.Customer.name} reagendou uma consulta com você. <br><br>" +
                        $"Data: {entity.booking_date.ToString("dd/MM/yyyy")} <br>" +
                        $"Hora: {entity.booking_date.ToString("HH:mm")}h <br> " +
                        $"Fuso Horário de SÃO PAULO",

                        nick = $"Voce tem uma consulta reagendada {entity.payment_status.dsPayment()}",
                        name = $"{entity.Provider.fantasy_name}",
                        email = entity.Provider.email,
                        title = $"reagendamento de consulta confirmada #{entity.id.ToString("000000")}"
                    });
                    await _appointmentRepository.RegisterLog(entity.id, $"Remarcação, Email enviado ao psico.");
                }
                catch { }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}