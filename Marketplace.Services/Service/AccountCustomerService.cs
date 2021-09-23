using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.account.customer;
using Marketplace.Domain.Models.Request.customers;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.account.customer;
using Marketplace.Domain.Models.Response.account.provider;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class AccountCustomerService
    {
        private readonly CustomAuthenticatedUser _authenticatedCustomer;
        private readonly AppointmentService _appointmentService;
        private readonly CustomerService _customerService;

        public AccountCustomerService(AppointmentService appointmentService,
                                      CustomerService customerService,
                                      CustomAuthenticatedUser user,
                                      ICustomCache cache)
        {
            _appointmentService = appointmentService;
            _customerService = customerService;
            _authenticatedCustomer = user;
        }

        public async Task<BaseRs<accountCustomerRs>> storeCustomer(accountCustomerRq _request)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                var _rq = new BaseRq<customerRq>()
                {
                    data = new customerRq()
                    {
                        name = _request.name.Clear().ToUpper(),
                        password = _request.password,
                        email = _request.email
                    }
                };

                // retorno customer
                var _resUpdate = await _customerService.Store(_rq);
                if (_resUpdate.error == null)
                    _res.content = new accountCustomerRs() { customer = _resUpdate.content };
                else
                    _res.error = _resUpdate.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountCustomerRs>> fetchAppointments(BaseRq<string> _request)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                var rq = new BaseRq<Domain.Models.Request.appointment.appointmentRq>()
                {
                    pagination = _request.pagination,
                    data = new Domain.Models.Request.appointment.appointmentRq()
                    {
                        customer_id = _authenticatedCustomer.user.id
                    }
                };

                var lst = await _appointmentService.showByCustomer(rq);
                if (lst.content.IsNotEmpty())
                {
                    _res.content = new accountCustomerRs();
                    _res.content.appointments = lst.content.ConvertAll(cc => new CustomerAppointment()
                    {
                        provider_name = $"{cc.Provider.fantasy_name} {cc.Provider.company_name}",
                        data = $"{cc.booking_date.ToString("dd/MM/yyyy")}",
                        hora = $"{cc.booking_date.ToString("HH:mm")}",
                        dsStatus = cc.status.ToString(),
                        id = cc.id
                    });
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountCustomerRs>> fetchAppointment(int id)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                var resApp = await _appointmentService.FindByAppointment(appointment_id: id);
                if (resApp.error == null && resApp.content != null)
                {
                    // apenas agendamento do cliente.
                    if (resApp.content.Customer.id == _authenticatedCustomer.user.id)
                    {
                        _res.content = new accountCustomerRs()
                        {
                            appointment = resApp.content
                        };
                    }
                }
                else
                    _res.error = resApp.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountCustomerRs>> fetchAppointmentInvoice(int id)
        {
            var _res = new BaseRs<accountCustomerRs>();
            try
            {
                var resApp = await _appointmentService.FindByAppointmentInvoice(appointment_id: id);
                if (resApp.error == null && resApp.content != null)
                {
                    // apenas agendamento do cliente.
                    if (resApp.content.Customer.id == _authenticatedCustomer.user.id)
                    {
                        _res.content = new accountCustomerRs()
                        {
                            appointment = resApp.content
                        };
                    }
                }
                else
                    _res.error = resApp.error;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<accountProviderRs>> findByUser()
        {
            return null;
            //var _res = new BaseRs<accountProviderRs>();
            //try
            //{
            //    _res.content = new accountProviderRs();
            //    _res.content.provider = (await _providerService.FindById(_authenticatedProvider.user.id)).content;
            //}
            //catch (System.Exception ex) { _res.setError(ex); }
            //return _res;
        }
    }
}