using AutoMapper;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.dto.provider;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.provider;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class BannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IConfiguration _configuration;
        private readonly ICustomCache _cache;

        private readonly UploadService _uploadService;

        public BannerService(IBannerRepository bannerRepository,
                             IConfiguration configuration,
                             ICustomCache cache,
                             UploadService uploadService)
        {
            _uploadService = uploadService;
            _bannerRepository = bannerRepository;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<BaseRs<List<Domain.Entities.Banner>>> Show(BaseRq<Domain.Entities.Banner> _request)
        {
            var _res = new BaseRs<List<Domain.Entities.Banner>>();
            try
            {
                _res.content = await _bannerRepository.Show(_request.pagination, _request.search);
                foreach(var item in _res.content)
                {
                    item.imageurl = item.image.toImageUrl($"{_configuration["storage:image"]}/banner");
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<Domain.Entities.Banner>> Store(BaseRq<Domain.Entities.Banner> _request)
        {
            var _res = new BaseRs<Domain.Entities.Banner>();
            try
            {
                await _bannerRepository.Create(_request.data);
                _res.content = _request.data;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<Domain.Entities.Banner>> Update(BaseRq<Domain.Entities.Banner> _request)
        {
            var _res = new BaseRs<Domain.Entities.Banner>();
            try
            {
                try
                {
                    var entity = await _bannerRepository.FindById(_request.data.id);
                    if (!_request.data.image.IsEmpty())
                    {
                        if(entity.image != _request.data.image)
                        {
                            if (entity.image.IsNotEmpty())
                                await _uploadService.RemoveImage(entity.image, "banner");
                        }
                    }

                    entity.image = _request.data.image;
                    await _bannerRepository.Update(entity);
                    _res.content = entity;
                }
                catch (System.Exception ex) { _res.setError(ex); }
                return _res;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<Domain.Entities.Banner>> FindById(int id)
        {
            var _res = new BaseRs<Domain.Entities.Banner>();
            try
            {
                var _banner = await _bannerRepository.FindById(id);
                if (_banner != null)
                    _banner.imageurl = _banner.image.toImageUrl($"{_configuration["storage:image"]}/banner");

                _res.content = _banner;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public List<Domain.Models.dto.Item> getSituations()
        {
            return new List<Domain.Models.dto.Item>()
            {
                new Domain.Models.dto.Item() { label = "Carrossel", value = ((int)Enumerados.BannerType.carrossel).ToString() },
                new Domain.Models.dto.Item() { label = "Avaliações", value = ((int)Enumerados.BannerType.assessment).ToString() },
            };
        }
    }
}