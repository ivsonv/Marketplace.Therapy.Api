using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models;
using Marketplace.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Marketplace.Infra.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly MarketPlaceContext _context;
        public BaseRepository(MarketPlaceContext context)
        {
            _context = context;
        }

        public IQueryable<T> Query => _context.Set<T>().AsNoTracking();
        public IQueryable<T> QueryTrack => _context.Set<T>();

        public void Add(T entity)
        {
            _context.Add<T>(entity);
        }

        public void AddRange(IList<T> entity)
        {
            _context.Set<T>().AddRange(entity);
        }

        public void Update(T entity)
        {
            _context.Update<T>(entity);
        }
        public void UpdateRange(IList<T> entity)
        {
            _context.UpdateRange(entity);
        }
        public async Task<T> Find(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }


        public void Remove(T entity)
        {
            _context.Remove<T>(entity);
        }
        public void RemoveRange(IList<T> entity)
        {
            _context.Set<T>().RemoveRange(entity);
        }

        public async Task SaveChanges()
        {
            var entries = _context.ChangeTracker
                                  .Entries()
                                  .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                                  .ToList();

            entries.ForEach(_entity =>
            {
                _entity.Property("updated_at").CurrentValue = CustomExtensions.DateNow;
                if (_entity.State == EntityState.Added)
                    _entity.Property("created_at").CurrentValue = _entity.Property("updated_at").CurrentValue;
                else
                    _entity.Property("created_at").IsModified = false;
            });
            await _context.SaveChangesAsync();
        }
        public async Task SaveHistory()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> Get(Pagination pagination)
            => this.Query.Skip(pagination.size * pagination.page).Take(pagination.size);

        public IQueryable<T> Get(IQueryable<T> query, Pagination pagination)
           => query.Skip(pagination.size * pagination.page).Take(pagination.size);

        public IQueryable<T> Get(IQueryable<T> query, Expression<Func<T, object>> order, Pagination pagination)
        {
            if (pagination.asc)
                return query.OrderBy(order)
                                     .Skip(pagination.size * pagination.page)
                                     .Take(pagination.size);
            else
                return query.OrderByDescending(order)
                                     .Skip(pagination.size * pagination.page)
                                     .Take(pagination.size);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
            => this.Query.Where(predicate);

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate, Pagination pagination)
            => this.Query.Where(predicate).Skip(pagination.size * pagination.page).Take(pagination.size);

        public IQueryable<T> Get(Expression<Func<T, object>> order, Pagination pagination)
        {
            if (pagination.asc)
                return this.Query.OrderBy(order)
                                     .Skip(pagination.size * pagination.page)
                                     .Take(pagination.size);
            else
                return this.Query.OrderByDescending(order)
                                     .Skip(pagination.size * pagination.page)
                                     .Take(pagination.size);

        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> order, Pagination pagination)
        {
            if (pagination.asc)
                return this.Query.Where(predicate)
                                     .OrderBy(order)
                                     .Skip(pagination.size * pagination.page)
                                     .Take(pagination.size);
            else
                return this.Query.Where(predicate)
                                     .OrderByDescending(order)
                                     .Skip(pagination.size * pagination.page)
                                     .Take(pagination.size);
        }
    }

    public interface IBaseRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<T> Find(int id);
        Task<T> Find(Expression<Func<T, bool>> predicate);
        void RemoveRange(IList<T> entity);
        void AddRange(IList<T> entity);
        Task SaveChanges();

        IQueryable<T> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> order, Pagination pagination);
        IQueryable<T> Get(Expression<Func<T, bool>> predicate, Pagination pagination);
        IQueryable<T> Get(Expression<Func<T, object>> order, Pagination pagination);
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> Get(Pagination pagination);
    }
}
