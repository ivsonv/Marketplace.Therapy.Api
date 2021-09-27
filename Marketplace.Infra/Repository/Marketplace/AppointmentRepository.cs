using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Interface.Shared;
using Marketplace.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Infra.Repository.Marketplace
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly BaseRepository<Appointment> _repository;
        private readonly ICustomCache _cache;

        public AppointmentRepository(BaseRepository<Appointment> repository,
                               ICustomCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<Appointment>> Show(Pagination pagination, string search = "")
        {
            //return await _repository.Get(order: o => o.id, pagination)
            //                        .Where(w => search.IsEmpty() || w.name.ToLower().Contains(search.ToLower()))
            //                        .Select(s => new Topic()
            //                        {
            //                            active = s.active,
            //                            name = s.name,
            //                            id = s.id,
            //                        }).ToListAsync();
            throw new System.NotImplementedException();
        }
        public Task<List<Appointment>> Show(Pagination pagination)
        {
            return this.Show(pagination);
        }
        public async Task<List<Appointment>> ShowByCustomer(Pagination pagination, int customer_id)
        {
            return await _repository.Query
                    .Include(i => i.Provider)
                    .Where(w => w.status == Enumerados.AppointmentStatus.confirmed)
                    .Where(w => w.customer_id == customer_id)
                    .Select(s => new Appointment()
                    {
                        Provider = new Provider() { fantasy_name = s.Provider.fantasy_name, company_name = s.Provider.company_name },
                        booking_date = s.booking_date,
                        status = s.status,
                        id = s.id
                    })
                    .OrderByDescending(o => o.booking_date)
                    .Skip(pagination.size * pagination.page).Take(pagination.size)
                    .ToListAsync();
        }
        public async Task<List<Appointment>> Show(Pagination pagination, int provider_id)
        {
            return await _repository.Query
                    .Include(i => i.Customer)
                    .Where(w => w.status == Enumerados.AppointmentStatus.confirmed)
                    .Where(w => w.provider_id == provider_id)
                    .Select(s => new Appointment()
                    {
                        Customer = new Customer() { name = s.Customer.name },
                        price_transfer = s.price_transfer,
                        booking_date = s.booking_date,
                        type = s.type,
                        id = s.id
                    })
                    .OrderByDescending(o => o.booking_date)
                    .Skip(pagination.size * pagination.page).Take(pagination.size)
                    .ToListAsync();
        }

        public async Task<Appointment> FindById(int id)
        {
            return await _repository.Query
                .Include(i => i.Provider).ThenInclude(t => t.SplitAccounts)
                .FirstOrDefaultAsync(f => f.id == id);
        }
        public async Task Create(Appointment entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }
        public async Task Update(Appointment entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }
        public async Task Delete(Appointment entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }
        public Task Delete(List<Appointment> entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Appointment> FindByAppointmentDetails(int appointment_id)
        {
            return await _repository.Query
                                    .Include(i => i.Provider)
                                    .Include(i => i.Customer)
                                    .Where(w => w.id == appointment_id)
                                    .Select(s => new Appointment()
                                    {
                                        Provider = new Provider() { fantasy_name = s.Provider.fantasy_name, company_name = s.Provider.company_name, id = s.provider_id },
                                        Customer = new Customer() { name = s.Customer.name, id = s.Customer.id },
                                        payment_status = s.payment_status,
                                        booking_date = s.booking_date,
                                        status = s.status
                                    }).FirstOrDefaultAsync();
        }
        public async Task<Appointment> FindByAppointmentInvoice(int appointment_id)
        {
            return await _repository.Query
                                    .Include(i => i.Provider).ThenInclude(t => t.Receipts)
                                    .Include(i => i.Customer)
                                    .FirstOrDefaultAsync(w => w.id == appointment_id);
        }

        public async Task<List<Appointment>> Reports(Pagination pagination, string term, DateTime dtStart, DateTime dtEnd, int provider_id)
        {
            var query = _repository.Query
                        .Include(i => i.Customer)
                        .Where(w => w.provider_id == provider_id);

            if (term.IsNotEmpty())
                query = query.Where(w => w.Customer.name.ToLower().Contains(term.ToLower()));

            if (dtStart != DateTime.MinValue)
                query = query.Where(w => w.booking_date.Date >= dtStart.Date);

            if (dtEnd != DateTime.MinValue)
                query = query.Where(w => w.booking_date.Date <= dtEnd.Date);

            // appointment
            return await query.Select(s => new Appointment()
            {
                Customer = new Customer() { name = s.Customer.name },
                price_transfer = s.price_transfer,
                booking_date = s.booking_date,
                id = s.id
            }).Skip(pagination.size * pagination.page).Take(pagination.size)
              .AsNoTracking().ToListAsync();

        }
    }
}