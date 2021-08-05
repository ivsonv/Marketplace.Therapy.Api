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
    public class GroupPermissionRepository : IGroupPermissionRepository
    {
        private readonly BaseRepository<GroupPermission> _repository;

        public GroupPermissionRepository(BaseRepository<GroupPermission> repository)
        {
            _repository = repository;
        }

        public async Task<List<GroupPermission>> Show(Pagination pagination)
        {
            return await _repository.Get(order: o => o.id, pagination)
                                    .Select(s => new GroupPermission()
                                    {                                    
                                        name = s.name,
                                        id = s.id,
                                    }).ToListAsync();
        }

        public async Task Create(GroupPermission entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();
        }

        public async Task Update(GroupPermission entity)
        {
            _repository.Update(entity);
            await _repository.SaveChanges();
        }

        public async Task Delete(GroupPermission entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<GroupPermission> FindById(int id)
        {
            return await _repository.Find(id);
        }
    }
}