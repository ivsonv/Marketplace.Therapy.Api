using Marketplace.Domain.Entities;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.topics;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.topics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class FaqQuestionService
    {
        private readonly IFaqQuestionRepository _questionRepository;
        private readonly ICustomCache _cache;

        public FaqQuestionService(IFaqQuestionRepository repository,
                          ICustomCache cache)
        {
            _questionRepository = repository;
            _cache = cache;
        }

        public async Task<BaseRs<List<FaqQuestion>>> show(BaseRq<FaqQuestion> _request)
        {
            var _res = new BaseRs<List<FaqQuestion>>();
            try
            {
                _res.content = await _questionRepository.Show(_request.pagination, _request.search);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<FaqQuestion>> Store(BaseRq<FaqQuestion> _request)
        {
            var _res = new BaseRs<FaqQuestion>();
            try
            {
                await _questionRepository.Create(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<FaqQuestion>> Update(BaseRq<FaqQuestion> _request)
        {
            var _res = new BaseRs<FaqQuestion>();
            try
            {
                await _questionRepository.Update(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<FaqQuestion>> FindById(int id)
        {
            var _res = new BaseRs<FaqQuestion>();
            try
            {
                _res.content = await _questionRepository.FindById(id);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<bool>> Delete(int id)
        {
            var _res = new BaseRs<bool>();
            try
            {
                await _questionRepository.Delete(await _questionRepository.FindById(id));
                _res.content = true;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}