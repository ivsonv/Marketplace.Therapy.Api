using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Infra.caching
{
    public class CacheApp : Domain.Interface.Integrations.caching.ICustomCache
    {
        private readonly Context.MarketPlaceContext _context;
        private readonly IMemoryCache _cache;
        private readonly int _minutes = 5;

        public CacheApp(IMemoryCache memoryCache, Context.MarketPlaceContext context)
        {
            _cache = memoryCache;
            _context = context;
        }

        public void Clear()
        {
            _cache.Remove("banks");
            _cache.Remove("languages");
            _cache.Remove("topics");
        }

        public async Task<List<Bank>> GetBanks()
        {
            return await _cache.GetOrCreateAsync("banks", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes((_minutes * 10));
                return await _context.Banks
                               .Select(s => new Domain.Entities.Bank()
                               {
                                   active = s.active,
                                   code = s.code,
                                   name = s.name,
                                   id = s.id
                               }).AsNoTracking().ToListAsync();
            });
        }
        public async Task<List<Language>> GetLanguages()
        {
            return await _cache.GetOrCreateAsync("languages", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes((_minutes * 6));
                return await _context.Languages
                               .Select(s => new Domain.Entities.Language()
                               {
                                   active = s.active,
                                   name = s.name,
                                   id = s.id
                               }).AsNoTracking().ToListAsync();
            });
        }
        public async Task<List<Topic>> GetTopics()
        {
            return await _cache.GetOrCreateAsync("topics", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes((_minutes * 6));
                return await _context.Topics
                               .Select(s => new Domain.Entities.Topic()
                               {
                                   active = s.active,
                                   name = s.name,
                                   id = s.id
                               }).AsNoTracking().ToListAsync();
            });
        }
    }
}