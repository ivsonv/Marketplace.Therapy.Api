using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface;
using Marketplace.Domain.Interface.Integrations.caching;
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
        private readonly ICustomCache _cache;

        public LanguageRepository(BaseRepository<Language> repository,
                                  ICustomCache cache)
        {
            _repository = repository;
            _cache = cache;
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

        public async Task<List<Language>> ShowCache(Pagination pagination, string search = "")
        {
            return (await _cache.GetLanguages())
                                .Where(w => search.IsEmpty() || w.name.ToLower().Contains(search.ToLower()))
                                .OrderBy(o => o.name)
                                .Skip(pagination.size * pagination.page)
                                .Take(pagination.size).ToList();
        }

        public async Task Create(Language entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task Update(Language entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task Delete(Language entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task<Language> FindById(int id)
        {
            return await _repository.Find(id);
        }

        public Task<List<Language>> Show(Pagination pagination, string seach = "")
        {
            return this.Show(pagination);
        }

        public async Task Delete(List<Language> entity)
        {
            _repository.RemoveRange(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }
    }
}