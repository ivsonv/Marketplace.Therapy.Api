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
    public class ProviderRepository : IProviderRepository
    {
        private readonly BaseRepository<Provider> _repository;

        public ProviderRepository(BaseRepository<Provider> repository)
        {
            _repository = repository;
        }

        public async Task<List<Provider>> Show(Pagination pagination)
        {
            return await _repository.Get(order: o => o.id, pagination)
                                    .Select(s => new Provider()
                                    {
                                        fantasy_name = s.fantasy_name,
                                        company_name = s.company_name,
                                        email = s.email,
                                        cnpj = s.cnpj,
                                        id = s.id
                                    }).ToListAsync();
        }

        public async Task<Provider> FindById(int id)
        {
            return await _repository.Query
                                    .Include(i => i.Address)
                                    .Include(i => i.BankAccounts)
                                    .FirstOrDefaultAsync(f => f.id == id);
        }

        public async Task Create(Provider entity)
        {
            this.formatData(entity);

            _repository.Add(entity);
            await _repository.SaveChanges();
        }
        private void formatData(Provider entity)
        {
            if (!entity.fantasy_name.IsEmpty()) entity.fantasy_name = entity.fantasy_name.Clear().ToUpper();
            if (!entity.company_name.IsEmpty()) entity.company_name = entity.company_name.Clear().ToUpper();
            if (!entity.nickname.IsEmpty()) entity.nickname = entity.nickname.Clear().ToUpper();
        }

        public async Task Update(Provider entity)
        {
            this.formatData(entity);

            _repository.Update(entity);
            await _repository.SaveChanges();
        }

        public async Task Delete(Provider entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<Provider> FindAuthByEmail(string email)
        {
            return await _repository.Get(g => g.email == email)
                  .Select(s => new Provider()
                  {
                      fantasy_name = s.fantasy_name,
                      id = s.id
                  }).FirstOrDefaultAsync();
        }

        public async Task<Provider> FindByEmail(string email)
            => await _repository.Query.FirstOrDefaultAsync(f => f.email == email);

        public async Task<Provider> FindByCnpj(string cnpj)
            => await _repository.Query.FirstOrDefaultAsync(f => f.cnpj == cnpj);
    }
}