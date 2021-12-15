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
    public class FaqQuestionRepository : IFaqQuestionRepository
    {
        private readonly BaseRepository<FaqQuestion> _repository;
        private readonly ICustomCache _cache;

        public FaqQuestionRepository(BaseRepository<FaqQuestion> repository,
                                     ICustomCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<FaqQuestion>> Show(Pagination pagination, string search = "0")
        {
            return await _repository.Get(order: o => o.id, pagination)
                                    .Where(w => w.faq_id == int.Parse(search))
                                    .Select(s => new FaqQuestion()
                                    {
                                        question = s.question,
                                        ans = s.ans,
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

        public Task<List<FaqQuestion>> Show(Pagination pagination)
        {
            return this.Show(pagination);
        }

        public async Task Create(FaqQuestion entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task Update(FaqQuestion entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();

            _cache.Clear();
        }

        public async Task Delete(FaqQuestion entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<FaqQuestion> FindById(int id)
        {
            return await _repository.Query.Include(i => i.Faq).FirstOrDefaultAsync(f => f.id == id);
        }

        public async Task Delete(List<FaqQuestion> entity)
        {
            _repository.RemoveRange(entity);
            await _repository.SaveChanges();
        }
    }
}