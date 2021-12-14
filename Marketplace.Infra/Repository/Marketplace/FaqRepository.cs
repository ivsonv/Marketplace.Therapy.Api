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
    public class FaqRepository : IFaqRepository
    {
        private readonly BaseRepository<Faq> _repository;
        private readonly ICustomCache _cache;

        public FaqRepository(BaseRepository<Faq> repository,
                               ICustomCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<Faq>> Show(Pagination pagination, string search = "")
        {
            return await _repository.Get(order: o => o.id, pagination)
                                    .Where(w => search.IsEmpty() || w.title.ToLower().Contains(search.ToLower()))
                                    .Select(s => new Faq()
                                    {
                                        sub_title = s.sub_title,
                                        title = s.title,
                                        id = s.id,
                                    }).ToListAsync();
        }

        public async Task<List<Faq>> ShowCache(Pagination pagination, string search = "")
        {
            return null;

            //return (await _cache.GetTopics())
            //                    .Where(w => search.IsEmpty() || w.name.ToLower().Contains(search.ToLower()))
            //                    .OrderBy(o => o.name)
            //                    .Skip(pagination.size * pagination.page)
            //                    .Take(pagination.size)
            //                    .ToList();
        }

        public Task<List<Faq>> Show(Pagination pagination)
        {
            return this.Show(pagination);
        }

        public async Task Create(Faq entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task Update(Faq entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task Delete(Faq entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task<Faq> FindById(int id)
        {
            return await _repository.Find(id);
        }

        public Task Delete(List<Faq> entity)
        {
            throw new System.NotImplementedException();
        }
    }
}