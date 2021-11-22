using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Infra.Repository.Marketplace
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BaseRepository<Customer> _repository;
        private readonly BaseRepository<Appointment> _repositoryAppointment;

        public CustomerRepository(BaseRepository<Appointment> repositoryAppointment,
                                  BaseRepository<Customer> repository)
        {
            _repositoryAppointment = repositoryAppointment;
            _repository = repository;
        }

        public async Task<List<Customer>> Show(Pagination pagination)
        {
            return await _repository.Get(order: o => o.id, pagination)
                                    .Select(s => new Customer()
                                    {
                                        email = s.email,
                                        cnpj = s.cnpj,
                                        name = s.name,
                                        cpf = s.cpf,
                                        id = s.id
                                    }).ToListAsync();
        }
        public async Task Create(Customer entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();
        }
        public async Task Update(Customer entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();
        }
        public async Task Delete(Customer entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<List<Appointment>> ShowAppointments(int customer_id)
        {
            return await _repositoryAppointment.Query
                .Include(i => i.Provider)
                .Where(w => w.customer_id == customer_id)
                .OrderByDescending(o => o.booking_date)
                .Select(s => new Appointment()
                {
                    Provider = new Provider() { fantasy_name = s.Provider.fantasy_name, company_name = s.Provider.company_name },
                    transaction_code = s.transaction_code,
                    payment_status = s.payment_status,
                    booking_date = s.booking_date,
                    created_at = s.created_at,
                    price = s.price,
                    id = s.id,
                }).ToListAsync();
        }

        public async Task<Customer> FindById(int id)
        {
            return await _repository.Query.FirstOrDefaultAsync(f => f.id == id);
        }
        public async Task<Customer> FindByEmail(string email)
        {
            return await _repository.Query.FirstOrDefaultAsync(f => f.email == email);
        }
        public async Task<Customer> FindByToken(string token)
        {
            return await _repository.Query.FirstOrDefaultAsync(f => f.recoverpassword != null && f.recoverpassword == token);
        }
        public async Task<Customer> FindAuthByEmail(string email)
        {
            return await _repository.Get(g => g.email == email)
                .Select(s => new Customer()
                {
                    password = s.password,
                    name = s.name,
                    id = s.id
                }).FirstOrDefaultAsync();
        }
        public async Task<List<Customer>> Show(Pagination pagination, string seach = "")
        {
            var query = _repository.Query.Select(s => new Customer()
            {
                email = s.email,
                cnpj = s.cnpj,
                name = s.name,
                cpf = s.cpf,
                id = s.id
            });

            seach = seach.Replace("null", "");
            if (seach.IsNotEmpty())
                query = query.Where(w => seach.IsEmpty() ||
                                               w.name.ToLower().Contains(seach.ToLower()) ||
                                               w.cpf != null && w.cpf.ToLower().Contains(seach.ToLower()) ||
                                               w.cnpj != null && w.cnpj.ToLower().Contains(seach.ToLower()) ||
                                               w.email.ToLower().Contains(seach.ToLower()));
            // consultar
            return await query
                .Skip(pagination.size * pagination.page)
                .Take(pagination.size)
                .OrderByDescending(o => o.id)
                .ToListAsync();
        }

        public Task Delete(List<Customer> entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Customer> FindByCpf(string cpf)
        {
            return await _repository.Query.FirstOrDefaultAsync(f => f.cpf == cpf);
        }
    }
}