using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Infra.Repository.Marketplace
{
    public class BannerRepository : IBannerRepository
    {
        private readonly BaseRepository<Banner> _repository;

        public BannerRepository(BaseRepository<Banner> repository)
        {
            _repository = repository;
        }

        public async Task<List<Banner>> Show(Pagination pagination, string seach = "")
        {
            return await _repository.Get(order: o => o.id, pagination).ToListAsync();
        }

        public async Task Create(Banner entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();
        }

        public async Task Delete(Banner entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public Task Delete(List<Banner> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Banner> FindById(int id) =>
            await _repository.Find(id);

        public Task<List<Banner>> Show(Pagination pagination)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Banner entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();
        }
    }
}