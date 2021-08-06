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
    public class TopicRepository : ITopicRepository
    {
        private readonly BaseRepository<Topic> _repository;

        public TopicRepository(BaseRepository<Topic> repository)
        {
            _repository = repository;
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
        public Task<List<Topic>> Show(Pagination pagination)
        {
            return this.Show(pagination);
        }

        public async Task Create(Topic entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();
        }

        public async Task Update(Topic entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();
        }

        public async Task Delete(Topic entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<Topic> FindById(int id)
        {
            return await _repository.Find(id);
        }

    }
}