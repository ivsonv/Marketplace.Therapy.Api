using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.Categories;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<BaseRs<List<categoryRs>>> show(BaseRq<categoryRq> _request)
        {
            var _res = new BaseRs<List<categoryRs>>();
            try
            {
                var lst = await _categoryRepository.Show(_request.pagination);
                _res.content = lst.ConvertAll(cc => new categoryRs() { id = cc.id, name = cc.name });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<categoryRs>> Store(BaseRq<categoryRq> _request)
        {
            var _res = new BaseRs<categoryRs>();
            try
            {
                var entity = new Domain.Entities.Category()
                {
                    name = _request.data.name,
                    active = true
                };
                await _categoryRepository.Create(entity);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<categoryRs>> Update(BaseRq<categoryRq> _request)
        {
            var _res = new BaseRs<categoryRs>();
            try
            {
                await _categoryRepository.Update(new Domain.Entities.Category()
                {
                    name = _request.data.name,
                    id = _request.data.id
                });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<categoryRs>> FindById(int id)
        {
            var _res = new BaseRs<categoryRs>();
            try
            {
                var entity = await _categoryRepository.FindById(id);
                if (entity != null)
                {
                    _res.content = new categoryRs();
                    _res.content.name = entity.name;
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
                await _categoryRepository.Delete(await _categoryRepository.FindById(id));
                _res.content = true;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}