using Marketplace.Domain.Entities;
using Marketplace.Domain.Interface;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Infra.Repository.Marketplace
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly BaseRepository<Language> _repository;

        public LanguageRepository(BaseRepository<Language> repository)
        {
            _repository = repository;
        }

        public async Task<List<Language>> Show(Pagination pagination)
        {
            return await _repository.Get(order: o => o.id, pagination)
                                    .Select(s => new Language()
                                    {
                                        active = s.active,
                                        name = s.name,
                                        id = s.id,
                                    }).ToListAsync();
        }

        public async Task Create(Language entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();
        }

        public async Task Update(Language entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();
        }

        public async Task Delete(Language entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<Language> FindById(int id)
        {
            return await _repository.Find(id);
        }
    }
}