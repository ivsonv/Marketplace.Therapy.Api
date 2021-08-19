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
    public class ProviderScheduleRepository : IProviderScheduleRepository
    {
        private readonly BaseRepository<ProviderSchedule> _repository;

        public ProviderScheduleRepository(BaseRepository<ProviderSchedule> repository)
        {
            _repository = repository;
        }

        public async Task<List<ProviderSchedule>> Show(Pagination pagination, string search = "")
        {
            return null;
        }

        public async Task<List<ProviderSchedule>> Show(Pagination pagination)
        {
            return await this.Show(pagination);
        }

        public async Task<ProviderSchedule> FindById(int id)
        {
            return await _repository.Query.FirstOrDefaultAsync(f => f.id == id);
        }

        public async Task Create(ProviderSchedule entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();
        }
        public async Task Update(ProviderSchedule entity)
        {
            var _current = await this.FindById(entity.id);
            if (_current != null)
            {
                _current.day_week = entity.day_week;
                _current.start = entity.start;
                _current.end = entity.end;
            }

            _repository.Update(_current);
            await _repository.SaveChanges();
        }
        public async Task Delete(ProviderSchedule entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public Task Delete(List<ProviderSchedule> entity)
        {
            throw new System.NotImplementedException();
        }
    }
}