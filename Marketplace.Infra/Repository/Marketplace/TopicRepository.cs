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
    public class TopicRepository : ITopicRepository
    {
        private readonly BaseRepository<Topic> _repository;
        private readonly ICustomCache _cache;

        public TopicRepository(BaseRepository<Topic> repository,
                               ICustomCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<Topic>> Show(Pagination pagination, string search = "")
        {
            return await _repository.Get(order: o => o.id, pagination)
                                    .Where(w => search.IsEmpty() || w.name.ToLower().Contains(search.ToLower()))
                                    .Select(s => new Topic()
                                    {
                                        active = s.active,
                                        name = s.name,
                                        id = s.id,
                                    }).ToListAsync();
        }

        public async Task<List<Topic>> ShowCache(Pagination pagination, string search = "")
        {
            return (await _cache.GetTopics())
                                .Where(w => search.IsEmpty() || w.name.ToLower().Contains(search.ToLower()))
                                .OrderBy(o => o.name)
                                .Skip(pagination.size * pagination.page)
                                .Take(pagination.size)
                                .ToList();
        }

        public Task<List<Topic>> Show(Pagination pagination)
        {
            return this.Show(pagination);
        }

        public async Task Create(Topic entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task Update(Topic entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task Delete(Topic entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task<Topic> FindById(int id)
        {
            return await _repository.Find(id);
        }

        public Task Delete(List<Topic> entity)
        {
            throw new System.NotImplementedException();
        }
    }
}