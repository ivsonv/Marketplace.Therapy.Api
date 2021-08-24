﻿using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Interface.Shared;
using Marketplace.Domain.Models;
using Microsoft.EntityFrameworkCore;
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
                    .OrderBy(o => o.booking_date)
                    .Skip(pagination.size * pagination.page).Take(pagination.size)
                    .ToListAsync();
        }

        public async Task<Appointment> FindById(int id)
        {
            return await _repository.Find(id);
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
    }
}