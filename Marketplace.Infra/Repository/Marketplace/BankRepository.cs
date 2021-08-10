using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Infra.Repository.Marketplace
{
    public class BankRepository : IBankRepository
    {
        private readonly BaseRepository<Bank> _repository;

        public BankRepository(BaseRepository<Bank> repository)
        {
            _repository = repository;
        }

        public async Task<List<Bank>> Show(Pagination pagination, string search = "")
        {
            return await _repository.Get(order: o => o.name, pagination)
                                    .Where(w => search.IsEmpty() || w.name.ToLower().Contains(search.ToLower()) || w.code.ToLower().Contains(search.ToLower()))
                                    .Select(s => new Bank()
                                    {
                                        active = s.active,
                                        code = s.code,
                                        name = s.name,
                                        id = s.id,
                                    }).ToListAsync();
        }
        public Task<List<Bank>> Show(Pagination pagination)
        {
            return this.Show(pagination);
        }

        public async Task Create(Bank entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();
        }

        public async Task Update(Bank entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();
        }

        public async Task Delete(Bank entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<Bank> FindById(int id)
        {
            return await _repository.Find(id);
        }
    }
}