using AutoMapper;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.groupPermissions;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.groupPermissions;
using Marketplace.Domain.Models.Response.topics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class GroupPermissionService
    {
        private readonly IGroupPermissionRepository _groupPermission;
        private readonly IMapper _mapper;

        public GroupPermissionService(IGroupPermissionRepository groupPermission,
                                      IMapper mapper)
        {
            _groupPermission = groupPermission;
            _mapper = mapper;
        }

        public async Task<BaseRs<List<groupPermissionRs>>> show(BaseRq<groupPermissionRq> _request)
        {
            var _res = new BaseRs<List<groupPermissionRs>>();
            try
            {
                var lst = await _groupPermission.Show(_request.pagination);
                _res.content = lst.ConvertAll(cc => new groupPermissionRs() { id = cc.id, name = cc.name});
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<groupPermissionRs>> Store(BaseRq<groupPermissionRq> _request)
        {
            var _res = new BaseRs<groupPermissionRs>();
            try
            {
                await _groupPermission.Create(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<groupPermissionRs>> Update(BaseRq<groupPermissionRq> _request)
        {
            var _res = new BaseRs<groupPermissionRs>();
            try
            {
                await _groupPermission.Update(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<groupPermissionRs>> FindById(int id)
        {
            var _res = new BaseRs<groupPermissionRs>();
            try
            {
                var entity = await _groupPermission.FindById(id);
                if (entity != null)
                {
                    _res.content = _mapper.Map<groupPermissionRs>(entity);
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
                await _groupPermission.Delete(await _groupPermission.FindById(id));
                _res.content = true;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}