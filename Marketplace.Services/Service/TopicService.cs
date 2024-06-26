﻿using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.topics;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.topics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class TopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ICustomCache _cache;

        public TopicService(ITopicRepository topicRepository,
                            ICustomCache cache)
        {
            _topicRepository = topicRepository;
            _cache = cache;
        }

        public async Task<BaseRs<List<topicRs>>> show(BaseRq<topicRq> _request)
        {
            var _res = new BaseRs<List<topicRs>>();
            try
            {
                var lst = await _topicRepository.Show(_request.pagination, _request.search);
                _res.content = lst.ConvertAll(cc => new topicRs() { id = cc.id, name = cc.name, active = cc.active, experience = cc.experience });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<List<topicRs>>> showCache(BaseRq<topicRq> _request)
        {
            var _res = new BaseRs<List<topicRs>>();
            try
            {
                var lst = await _topicRepository.ShowCache(_request.pagination, _request.search);
                _res.content = lst.ConvertAll(cc => new topicRs() { id = cc.id, name = cc.name, active = cc.active, experience = cc.experience });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<topicRs>> Store(BaseRq<topicRq> _request)
        {
            var _res = new BaseRs<topicRs>();
            try
            {
                await _topicRepository.Create(new Domain.Entities.Topic()
                {
                    active = _request.data.active ?? false,
                    experience = _request.data.experience,
                    name = _request.data.name,
                    id = _request.data.id
                });
                _cache.Clear();
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<topicRs>> Update(BaseRq<topicRq> _request)
        {
            var _res = new BaseRs<topicRs>();
            try
            {
                await _topicRepository.Update(new Domain.Entities.Topic()
                {
                    active = _request.data.active ?? false,
                    experience = _request.data.experience,
                    name = _request.data.name,
                    id = _request.data.id
                });
                _cache.Clear();
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<topicRs>> FindById(int id)
        {
            var _res = new BaseRs<topicRs>();
            try
            {
                var entity = await _topicRepository.FindById(id);
                if (entity != null)
                {
                    _res.content = new topicRs()
                    {   
                        experience = entity.experience,
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
                await _topicRepository.Delete(await _topicRepository.FindById(id));
                _res.content = true;
                _cache.Clear();
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}