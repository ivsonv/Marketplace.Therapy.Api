﻿using Marketplace.Domain.Entities;
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
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _repository = appointmentRepository;
        }

        public async Task<BaseRs<appointmentRs>> Store(BaseRq<appointmentRq> _request)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                await _repository.Create(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<appointmentRs>> Update(BaseRq<appointmentRq> _request)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                await _repository.Update(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
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
        public async Task<BaseRs<List<appointmentRs>>> showByCustomer(BaseRq<appointmentRq> _request)
        {
            var _res = new BaseRs<List<appointmentRs>>();
            try
            {
                var lst = await _repository.ShowByCustomer(_request.pagination, _request.data.customer_id);
                _res.content = lst.ConvertAll(cc => new appointmentRs()
                {
                    Provider = cc.Provider,
                    booking_date = cc.booking_date,
                    status = cc.status,
                    id = cc.id
                });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}