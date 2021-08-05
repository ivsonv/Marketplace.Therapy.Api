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
        private readonly BaseRepository<User> _repository;

        public UserRepository(BaseRepository<User> repository)
        {
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
            _repository.Update(entity);
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
                                          .Select(s => new User()
                                          {
                                              id = s.id,
                                              name = s.name,
                                              email = s.email,
                                              active = s.active,
                                              password = s.password,
                                              GroupPermissions = s.GroupPermissions.Select(x => new UserGroupPermission()
                                              {
                                                  //GroupPermission = x.GroupPermission != null
                                                  //? new GroupPermission() { name = x.GroupPermission.name, id = x.GroupPermission.id }
                                                  //: null,
                                                  group_permission_id = x.group_permission_id,
                                                  user_id = x.user_id,
                                                  id = x.id
                                              })
                                          })
                                          .FirstOrDefaultAsync(f => f.id == id);
        }
    }
}