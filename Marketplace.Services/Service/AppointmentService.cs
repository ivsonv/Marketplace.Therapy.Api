using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.appointment;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.appointment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class AppointmentService
    {
        private readonly CustomAuthenticatedUser _authenticatedUser;
        private readonly IAppointmentRepository _repository;
        private readonly ICustomCache _cache;

        public AppointmentService(IAppointmentRepository appointmentRepository,
                                  CustomAuthenticatedUser user,
                                  ICustomCache cache)
        {
            _repository = appointmentRepository;
            _authenticatedUser = user;
            _cache = cache;
        }

        public async Task<BaseRs<List<appointmentRs>>> show(BaseRq<appointmentRq> _request)
        {
            var _res = new BaseRs<List<appointmentRs>>();
            try
            {
                var lst = await _repository.Show(_request.pagination, _request.search);
                _res.content = lst.ConvertAll(cc => new appointmentRs() { id = cc.id });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<List<appointmentRs>>> showByProvider(BaseRq<appointmentRq> _request)
        {
            var _res = new BaseRs<List<appointmentRs>>();
            try
            {
                var lst = await _repository.Show(_request.pagination, _request.data.provider_id);
                _res.content = lst.ConvertAll(cc => new appointmentRs()
                {
                    Customer = new Customer() { name = cc.Customer.name },
                    price_transfer = cc.price_transfer,
                    booking_date = cc.booking_date,
                    type = cc.type,
                    id = cc.id
                });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

    }
}