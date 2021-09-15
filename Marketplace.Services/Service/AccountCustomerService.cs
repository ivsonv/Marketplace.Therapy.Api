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
        private readonly CustomerService _customerService;

        public AccountCustomerService(CustomerService customerService,
                                      CustomAuthenticatedUser user,
                                      ICustomCache cache)
        {
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