using AutoMapper;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.users;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.account.provider;
using Marketplace.Domain.Models.Response.users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class AccountProviderService
    {
        private readonly CustomAuthenticatedUser _authenticatedUser;
        private readonly ProviderService _providerService;
        private readonly ICustomCache _cache;

        public AccountProviderService(ProviderService providerService,
                                      CustomAuthenticatedUser user,
                                      ICustomCache cache)
        {
            _providerService = providerService;
            _authenticatedUser = user;
            _cache = cache;
        }

        public async Task<BaseRs<accountProviderRs>> findByUser()
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                _res.content = new accountProviderRs();
                _res.content.provider = (await _providerService.FindById(_authenticatedUser.user.id)).content;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<accountProviderRs>> fetchLanguages()
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                _res.content = new accountProviderRs()
                {
                    languages = await _cache.GetLanguages()
                };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountProviderRs>> fetchTopics()
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                _res.content = new accountProviderRs()
                {
                    topics = await _cache.GetTopics()
                };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<accountProviderRs>> fetchBanks(string _term)
        {
            var _res = new BaseRs<accountProviderRs>();
            try
            {
                _res.content = new accountProviderRs()
                {
                    banks = (await _cache.GetBanks()).Where(w => _term.IsEmpty() ||
                                                            w.name.IsCompare().Contains(_term.IsCompare()) ||
                                                            w.code.IsCompare().Contains(_term.IsCompare()))
                                                     .Where(w => w.active)
                                                     .Select(s => new Domain.Models.Response.banks.bankRs()
                                                     {
                                                         code = s.code,
                                                         name = s.name
                                                     }).ToList()
                };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}