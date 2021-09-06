using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.marketplace;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.marketplace;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class MarketplaceService
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomCache _cache;

        public MarketplaceService(IConfiguration configuration, ICustomCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<BaseRs<List<providerMktRs>>> ShowProviders(BaseRq<providerMktRq> _request)
        {
            var _res = new BaseRs<List<providerMktRs>>();
            if (_request.data == null)
                _request.data = new providerMktRq();
            try
            {
                var list = await _cache.GetProviders();

                if (_request.data.name.IsNotEmpty())
                    list = list.Where(w => w.fantasy_name.IsCompare().Contains(_request.data.name.IsCompare()) ||
                                           w.company_name.IsCompare().Contains(_request.data.name.IsCompare()) ||
                                           w.nickname.IsCompare().Contains(_request.data.name.IsCompare())).ToList();

                // list
                _request.pagination.size = 20; //force
                _res.content = list
                    .Where(w => w.image.IsNotEmpty())
                    .OrderBy(o => o.fantasy_name)
                    .Skip(_request.pagination.size * _request.pagination.page)
                    .Take(_request.pagination.size)
                    .Select(x => new providerMktRs()
                    {
                        name = x.nickname.IsNotEmpty() ? x.nickname : $"{x.fantasy_name} {x.company_name}",
                        image = x.image.toImageUrl($"{_configuration["storage:image"]}/profile"),
                        state = !x.Address.IsEmpty()
                        ? x.Address.First().uf : null,
                        introduction = x.description,
                        price = x.price,
                        link = x.link,
                        crp = x.crp
                    }).ToList();
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<providerMktRs>> FindByProvider(string link)
        {
            var _res = new BaseRs<providerMktRs>();
            try
            {
                var provider = (await _cache.GetProviders()).FirstOrDefault(f => f.link.IsCompare() == link.IsCompare());
                _res.content = new providerMktRs()
                {
                    name = provider.nickname.IsNotEmpty() ? provider.nickname : $"{provider.fantasy_name} {provider.company_name}",
                    image = provider.image.toImageUrl($"{_configuration["storage:image"]}/profile"),
                    state = !provider.Address.IsEmpty()
                        ? provider.Address.First().uf : null,
                    introduction = provider.description,
                    biography = provider.biography,
                    price = provider.price,
                    link = provider.link,
                    crp = provider.crp
                };

                // topics
                if (!provider.Topics.IsEmpty())
                {
                    // topics do provider
                    var ids = provider.Topics.Select(s => s.topic_id);

                    // pegar topics vinculado
                    var topics = (await _cache.GetTopics()).Where(w => w.active && ids.Any(a => a == w.id)).ToList();

                    // separar experiencia / especialidade
                    if (topics.Any())
                    {
                        // experiencia
                        if (topics.Any(a => a.experience))
                            _res.content.experiences = topics.Where(w => w.experience)
                                                             .Select(s => new providerMktItem()
                                                             {
                                                                 name = s.name
                                                             });

                        // especialidade
                        if (topics.Any(a => !a.experience))
                            _res.content.expertises = topics.Where(w => !w.experience)
                                                         .Select(s => new providerMktItem()
                                                         {
                                                             name = s.name
                                                         });
                    }
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}