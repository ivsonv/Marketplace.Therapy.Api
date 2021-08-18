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
                                   experience = s.experience,
                                   active = s.active,
                                   name = s.name,
                                   id = s.id
                               }).AsNoTracking().ToListAsync();
            });
        }

        public async Task<List<Appointment>> GetAppointments()
        {
            return await _cache.GetOrCreateAsync("appointments", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_minutes);
                return await _context.Appointments
                               .Select(s => new Appointment()
                               {
                                   payment_status = s.payment_status,
                                   booking_date = s.booking_date,
                                   provider_id = s.provider_id,
                                   id = s.id
                               })
                               .Where(w => w.payment_status == Domain.Helpers.Enumerados.PaymentStatus.confirmed)
                               .Where(w => w.booking_date >= DateTime.UtcNow)
                               .AsNoTracking().ToListAsync();
            });
        }
        public async Task<List<Provider>> GetProviders()
        {
            return await _cache.GetOrCreateAsync("providers", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_minutes);
                return await _context.Providers
                               .Select(s => new Provider()
                               {
                                   Languages = s.Languages.Any() ? s.Languages.Select(tt => new ProviderLanguages() { id = tt.id }) : null,
                                   Address = s.Address.Any() ? s.Address.Select(tt => new ProviderAddress() { uf = tt.uf }) : null,
                                   Topics = s.Topics.Any() ? s.Topics.Select(tt => new ProviderTopics() { id = tt.id }) : null,
                                   fantasy_name = s.fantasy_name,
                                   company_name = s.company_name,
                                   biography = s.biography,
                                   nickname = s.nickname,
                                   price = s.price,
                                   image = s.image,
                                   crp = s.crp,
                                   id = s.id
                               }).AsNoTracking().ToListAsync();
            });
        }
    }
}