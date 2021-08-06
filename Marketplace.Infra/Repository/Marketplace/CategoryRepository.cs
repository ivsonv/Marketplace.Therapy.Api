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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BaseRepository<Category> _repository;

        public CategoryRepository(BaseRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> Show(Pagination pagination)
        {
            return await _repository.Get(order: o => o.id, pagination)
                                    .Select(s => new Category()
                                    {
                                        active = s.active,
                                        name = s.name,
                                        id = s.id,
                                    }).ToListAsync();
        }

        public async Task Create(Category entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();
        }

        public async Task Update(Category entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();
        }

        public async Task Delete(Category entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<Category> FindById(int id)
        {
            return await _repository.Find(id);
        }

        public Task<List<Category>> Show(Pagination pagination, string seach = "")
        {
            throw new System.NotImplementedException();
        }
    }
}