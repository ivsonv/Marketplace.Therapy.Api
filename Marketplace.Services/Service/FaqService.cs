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
    public class FaqService
    {
        private readonly IFaqRepository _faqRepository;
        private readonly ICustomCache _cache;

        public FaqService(IFaqRepository topicRepository,
                          ICustomCache cache)
        {
            _faqRepository = topicRepository;
            _cache = cache;
        }

        public async Task<BaseRs<List<Faq>>> show(BaseRq<Faq> _request)
        {
            var _res = new BaseRs<List<Faq>>();
            try
            {
                _res.content = await _faqRepository.Show(_request.pagination, _request.search);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<List<Faq>>> showCache(BaseRq<Faq> _request)
        {
            var _res = new BaseRs<List<Faq>>();
            try
            {
                _res.content = await _faqRepository.ShowCache(_request.pagination, _request.search);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<Faq>> Store(BaseRq<Faq> _request)
        {
            var _res = new BaseRs<Faq>();
            try
            {
                await _faqRepository.Create(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<Faq>> Update(BaseRq<Faq> _request)
        {
            var _res = new BaseRs<Faq>();
            try
            {
                await _faqRepository.Update(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<Faq>> FindById(int id)
        {
            var _res = new BaseRs<Faq>();
            try
            {
                _res.content = await _faqRepository.FindById(id);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<bool>> Delete(int id)
        {
            var _res = new BaseRs<bool>();
            try
            {
                await _faqRepository.Delete(await _faqRepository.FindById(id));
                _res.content = true;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}