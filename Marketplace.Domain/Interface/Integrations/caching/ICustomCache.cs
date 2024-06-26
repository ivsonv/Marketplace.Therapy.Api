﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Integrations.caching
{
    public interface ICustomCache
    {
        void Clear(string key = "");
        Task<List<Entities.Bank>> GetBanks();
        Task<List<Entities.Topic>> GetTopics();
        Task<List<Entities.Provider>> GetProviders();
        Task<List<Entities.Language>> GetLanguages();
        List<Entities.GroupPermission> GetPermissions();
        Task<List<Entities.Appointment>> GetCalendar();
        Task<List<Entities.Appointment>> GetAppointmentsActive();
        Task<List<Entities.Appointment>> GetCalendar(int mes);
        Task<List<Entities.Faq>> GetFaq();
        Task<List<Entities.Banner>> GetBanners();
    }
}