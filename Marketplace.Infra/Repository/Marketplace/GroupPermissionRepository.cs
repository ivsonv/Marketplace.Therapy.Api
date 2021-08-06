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
        private readonly BaseRepository<GroupPermissionAttached> _repositoryAttached;

        public GroupPermissionRepository(BaseRepository<GroupPermission> repository, 
                                         BaseRepository<GroupPermissionAttached> repositoryAttached)
        {
            _repositoryAttached = repositoryAttached;
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
            var _current = await this.FindById(entity.id);
            if (_current != null)
            {
                #region ..: permissions :..

                var permissionReceives = entity.PermissionsAttached.Select(s => s.name).ToList();
                var permissionCurrents = _current.PermissionsAttached.Select(s => s.name).ToList();
                var permissionRemoves = permissionCurrents.Where(w => !permissionReceives.Contains(w)).ToList();
                if (permissionRemoves.Any())
                {
                    var _lst = _current.PermissionsAttached.Where(w => permissionRemoves.Contains(w.name)).ToList();
                    _repositoryAttached.RemoveRange(_lst);
                    await _repositoryAttached.SaveChanges();

                    _current.PermissionsAttached = null;
                }

                permissionReceives = permissionReceives.Where(w => !permissionCurrents.Contains(w)).ToList();
                if (permissionReceives.Any())
                    _current.PermissionsAttached = permissionReceives.Distinct().ToList().ConvertAll(c => new GroupPermissionAttached()
                    {
                        name = c
                    });
                #endregion

                _current.name = entity.name;
                _repository.Update(_current);
                await _repository.SaveChanges();
            }
        }

        public async Task Delete(GroupPermission entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<GroupPermission> FindById(int id)
        {
            return await _repository.Query
                                    .Include(i => i.PermissionsAttached)
                                    .Select(s => new GroupPermission()
                                    {
                                        name = s.name,
                                        id = s.id,
                                        PermissionsAttached = s.PermissionsAttached.Select(x => new GroupPermissionAttached()
                                        {
                                            group_permission_id = x.group_permission_id,
                                            name = x.name,
                                            id = x.id
                                        })
                                    })
                                    .FirstOrDefaultAsync(f => f.id == id);
        }

        public Task<List<GroupPermission>> Show(Pagination pagination, string seach = "")
        {
            throw new System.NotImplementedException();
        }
    }
}