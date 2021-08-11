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
    public class UserRepository : IUserRepository
    {
        private readonly BaseRepository<UserGroupPermission> _repositoryUserGroupPermission;
        private readonly BaseRepository<User> _repository;

        public UserRepository(BaseRepository<UserGroupPermission> repositoryUserGroupPermission,
                              BaseRepository<User> repository)
        {
            _repositoryUserGroupPermission = repositoryUserGroupPermission;
            _repository = repository;
        }

        public async Task<List<User>> Show(Pagination pagination)
        {
            return await _repository.Get(order: o => o.id, pagination)
                                    .Select(s => new User()
                                    {
                                        active = s.active,
                                        email = s.email,
                                        name = s.name,
                                        id = s.id,
                                    }).ToListAsync();
        }

        public async Task Create(User entity)
        {
            _repository.Add(entity);
            await _repository.SaveChanges();
        }

        public async Task Update(User entity)
        {
            var _current = await this.FindById(entity.id);

            #region ..: group de acess :..

            var usersReceives = entity.GroupPermissions.Select(s => s.group_permission_id).ToList();
            var usersCurrents = _current.GroupPermissions.Select(s => s.group_permission_id).ToList();
            var usersRemoves = usersCurrents.Where(w => !usersReceives.Contains(w)).ToList();
            if (usersRemoves.Any())
            {
                var lst = _current.GroupPermissions.Where(w => usersRemoves.Contains(w.group_permission_id)).ToList();
                _repositoryUserGroupPermission.RemoveRange(lst);
                await _repositoryUserGroupPermission.SaveChanges();
            }

            usersReceives = usersReceives.Where(w => !usersCurrents.Contains(w)).ToList();
            if (usersReceives.Any())
                _current.GroupPermissions = usersReceives.ConvertAll(c
                    => new UserGroupPermission()
                    {
                        group_permission_id = c
                    });
            #endregion

            _current.active = entity.active;
            _current.email = entity.email;
            _current.name = entity.name;
            _repository.Update(_current);
            await _repository.SaveChanges();
        }

        public async Task Delete(User entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<User> FindById(int id)
        {
            return await _repository.Query.Include(i => i.GroupPermissions)
                                              .ThenInclude(t => t.GroupPermission)
                                          .FirstOrDefaultAsync(f => f.id == id);
        }

        public Task<User> FindByEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> FindAuthByEmail(string email)
        {
            return await _repository.Query.Include(i => i.GroupPermissions)
                                            .ThenInclude(t => t.GroupPermission)
                                            .ThenInclude(t => t.PermissionsAttached)
                                          .Select(s => new User()
                                          {
                                              id = s.id,
                                              name = s.name,
                                              email = s.email,
                                              active = s.active,
                                              password = s.password,
                                              GroupPermissions = s.GroupPermissions.Select(x => new UserGroupPermission()
                                              {
                                                  group_permission_id = x.group_permission_id,
                                                  GroupPermission = x.GroupPermission,
                                                  user_id = x.user_id,
                                                  id = x.id
                                              })
                                          })
                                          .FirstOrDefaultAsync(f => f.email == email);
        }

        public Task<List<User>> Show(Pagination pagination, string seach = "")
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(List<User> entity)
        {
            throw new System.NotImplementedException();
        }
    }
}