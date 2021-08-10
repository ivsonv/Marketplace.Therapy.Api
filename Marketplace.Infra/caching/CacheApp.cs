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
        public CacheApp(IMemoryCache memoryCache, Context.MarketPlaceContext context)
        {
            _cache = memoryCache;
            _context = context;
        }

        public void Clear()
        {
            _cache.Remove("banks");
        }

        public async Task<List<Domain.Entities.Bank>> GetBanks()
        {
            return await _cache.GetOrCreateAsync("banks", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_minutes);
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

        private readonly IMemoryCache _cache;
        private readonly int _minutes = 5;

    }
}
