using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
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

        public void Clear(string key = "")
        {
            if (key.IsEmpty())
            {
                _cache.Remove("appointments");
                _cache.Remove("permissions");
                _cache.Remove("calendars");
                _cache.Remove("providers");
                _cache.Remove("languages");
                _cache.Remove("topics");
                _cache.Remove("banks");
            }
            else
                _cache.Remove(key);
        }

        public async Task<List<Provider>> GetProviders()
        {
            return await _cache.GetOrCreateAsync("providers", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_minutes);
                return await _context.Providers
                               .Include(i => i.Schedules)
                               .Include(i => i.Address)
                               .Include(i => i.Topics)
                               .OrderBy(o => o.order)
                               .Select(s => new Provider()
                               {
                                   Languages = s.Languages.Any() ? s.Languages.Select(tt => new ProviderLanguages() { language_id = tt.language_id }) : null,
                                   Topics = s.Topics.Any() ? s.Topics.Select(tt => new ProviderTopics() { topic_id = tt.topic_id }) : null,
                                   Address = s.Address.Any() ? s.Address.Select(tt => new ProviderAddress() { uf = tt.uf }) : null,
                                   completed = s.completed,
                                   fantasy_name = s.fantasy_name,
                                   company_name = s.company_name,
                                   description = s.description,
                                   biography = s.biography,
                                   nickname = s.nickname,
                                   youtube = s.youtube,
                                   active = s.active,
                                   price = s.price,
                                   image = s.image,
                                   link = s.link,
                                   crp = s.crp,
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
        public List<GroupPermission> GetPermissions()
        {
            return _cache.GetOrCreate("permissions", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_minutes * 100);
                return _context.GroupPermissions
                               .Include(i => i.PermissionsAttached)
                               .Select(s => new GroupPermission()
                               {
                                   name = s.name,
                                   id = s.id,
                                   PermissionsAttached = s.PermissionsAttached.Select(ss => new GroupPermissionAttached()
                                   {
                                       name = ss.name
                                   })
                               }).AsNoTracking().ToList();
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

        public async Task<List<Appointment>> GetAppointmentsActive()
        {
            return await _cache.GetOrCreateAsync("appointments", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_minutes * 20);
                return await _context.Appointments
                               .Where(w => w.payment_status == Enumerados.PaymentStatus.confirmed)
                               .Where(w => w.booking_date > CustomExtensions.DateNow)
                               .Select(s => new Appointment()
                               {
                                   booking_date = s.booking_date,
                                   provider_id = s.provider_id,
                               }).AsNoTracking().ToListAsync();
            });
        }
        public async Task<List<Appointment>> GetCalendar()
        {
            return await _cache.GetOrCreateAsync("calendars", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_minutes * 5);

                DateTime dt = DateTime.Parse($"{CustomExtensions.DateNow.Year}-{CustomExtensions.DateNow.Month}-01"); // dia primeiro do mes atual
                return await this.GetCalendar(dt, DateTime.MinValue);
            });
        }
        public async Task<List<Appointment>> GetCalendar(int mes)
        {
            DateTime dt = DateTime.Parse($"{CustomExtensions.DateNow.Year}-{mes.ToString("00")}-01");
            DateTime dtend = dt.AddMonths(1).AddDays(-1);
            return await this.GetCalendar(dt, dtend);
        }
        public async Task<List<Appointment>> GetCalendar(DateTime dtstart, DateTime dtend)
        {
            // query
            var query = _context.Appointments
                                .Include(i => i.Customer)
                                .Where(w => w.status == Enumerados.AppointmentStatus.confirmed)
                                .Where(w => w.booking_date >= dtstart);

            // fim
            if (dtend != DateTime.MinValue)
                query = query.Where(w => w.booking_date <= dtend);

            // list
            return await query.Select(s => new Appointment()
            {
                Customer = new Customer() { name = s.Customer.name },
                price_transfer = s.price_transfer,
                booking_date = s.booking_date,
                customer_id = s.customer_id,
                provider_id = s.provider_id,
                price_full = s.price_full,
                type = s.type,
                id = s.id
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<Faq>> GetFaq()
        {
            return await _cache.GetOrCreateAsync("faq", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return await _context.Faq
                               .Include(i => i.Question)
                               .Select(s => new Domain.Entities.Faq()
                               {
                                   title = s.title,
                                   sub_title = s.sub_title,
                                   id = s.id,
                                   Question = s.Question.Select(ss =>
                                   new FaqQuestion()
                                   {
                                       question = ss.question,
                                       ans = ss.ans
                                   }),

                               }).AsNoTracking().ToListAsync();
            });
        }

        public async Task<List<Banner>> GetBanners()
        {
            return await _cache.GetOrCreateAsync("banners", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await _context.Banners.AsNoTracking().ToListAsync();
            });
        }
    }
}