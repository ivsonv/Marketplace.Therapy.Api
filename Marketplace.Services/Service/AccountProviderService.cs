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
                _res.content.Provider = (await _providerService.FindById(_authenticatedUser.user.id)).content;
                //_res.content.languages = await _cache.GetLanguages();
                //_res.content.topics = await _cache.GetTopics();
                //_res.content.banks = await _cache.GetBanks();
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;

        }




    }
}