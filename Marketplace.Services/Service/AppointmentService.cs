using AutoMapper;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.appointment;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.appointment;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;


        public AppointmentService(IAppointmentRepository appointmentRepository,
                                  IConfiguration configuration,
                                  IMapper mapper)
        {
            _repository = appointmentRepository;
            _configuration = configuration;
            _mapper = mapper;
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
                if (lst.Any())
                {
                    _res.content = new List<appointmentRs>();
                    lst.ForEach(fe =>
                    {
                        _res.content.Add(_mapper.Map<appointmentRs>(fe));
                    });
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<appointmentRs>> FindByAppointment(int appointment_id)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                var app = await _repository.FindByAppointmentDetails(appointment_id: appointment_id);
                if (app != null)
                {
                    _res.content = _mapper.Map<appointmentRs>(app);
                    _res.content.dsStatusPayment = _res.content.payment_status.ToString();
                    _res.content.dsStatus = _res.content.status.ToString();
                    _res.content.start = _res.content.booking_date.ToString("dd/MM/yyyy");
                    _res.content.hour = _res.content.booking_date.TimeOfDay;
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<appointmentRs>> FindByAppointmentInvoice(int appointment_id)
        {
            var _res = new BaseRs<appointmentRs>();
            try
            {
                var app = await _repository.FindByAppointmentInvoice(appointment_id: appointment_id);
                if (app != null)
                {
                    _res.content = _mapper.Map<appointmentRs>(app);
                    _res.content.dsStatusPayment = _res.content.payment_status.ToString();
                    _res.content.dsStatus = _res.content.status.ToString();
                    _res.content.start = _res.content.booking_date.ToString("dd/MM/yyyy");
                    _res.content.hour = _res.content.booking_date.TimeOfDay;
                    _res.content.issued = Domain.Helpers.CustomExtensions.DateNow.ToString("dd/MM/yyyy");
                    _res.content.Provider.password = null;

                    _res.content.Provider.Receipts.First().signature = $"{_configuration["storage:image"]}/signature/{_res.content.Provider.Receipts.First().signature}";
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}