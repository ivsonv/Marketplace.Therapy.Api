using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.languages;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class LanguageService
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<BaseRs<List<languageRs>>> show(BaseRq<languageRq> _request)
        {
            var _res = new BaseRs<List<languageRs>>();
            try
            {
                var categories = await _languageRepository.Show(_request.pagination);
                _res.content = categories.ConvertAll(cc => new languageRs() { id = cc.id, name = cc.name, active = cc.active });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<languageRs>> Store(BaseRq<languageRq> _request)
        {
            var _res = new BaseRs<languageRs>();
            try
            {
                var entity = new Domain.Entities.Language()
                {
                    active = _request.data.active ?? false,
                    name = _request.data.name
                };
                await _languageRepository.Create(entity);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<languageRs>> Update(BaseRq<languageRq> _request)
        {
            var _res = new BaseRs<languageRs>();
            try
            {
                await _languageRepository.Update(new Domain.Entities.Language()
                {
                    active = _request.data.active ?? false,
                    name = _request.data.name,
                    id = _request.data.id
                });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<languageRs>> FindById(int id)
        {
            var _res = new BaseRs<languageRs>();
            try
            {
                var entity = await _languageRepository.FindById(id);
                if (entity != null)
                {
                    _res.content = new languageRs()
                    {
                        active = entity.active,
                        name = entity.name,
                        id = entity.id
                    };
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<bool>> Delete(int id)
        {
            var _res = new BaseRs<bool>();
            try
            {
                await _languageRepository.Delete(await _languageRepository.FindById(id));
                _res.content = true;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}