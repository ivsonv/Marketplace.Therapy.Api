using Marketplace.Domain.Entities;
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

        public CustomerRepository(BaseRepository<Customer> repository)
        {
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

        public async Task<Customer> FindById(int id)
        {
            return await _repository.Query.Include(i => i.Address).FirstOrDefaultAsync(f => f.id == id);
        }

        public async Task<Customer> FindByEmail(string email)
        {
            return await _repository.Query.FirstOrDefaultAsync(f => f.email == email);
        }

        public async Task<Customer> FindAuthByEmail(string email)
        {
            return await _repository.Get(g => g.email == email)
                .Select(s => new Customer()
                {
                    name = s.name,
                    id = s.id
                }).FirstOrDefaultAsync();
        }

        public Task<List<Customer>> Show(Pagination pagination, string seach = "")
        {
            throw new System.NotImplementedException();
        }
    }
}