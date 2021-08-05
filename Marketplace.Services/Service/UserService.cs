using AutoMapper;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.users;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
                           IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseRs<List<userRs>>> show(BaseRq<userRq> _request)
        {
            var _res = new BaseRs<List<userRs>>();
            try
            {
                var lst = await _userRepository.Show(_request.pagination);
                _res.content = lst.ConvertAll(cc => new userRs() { id = cc.id, name = cc.name, email = cc.email, active = cc.active });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<userRs>> Store(BaseRq<userRq> _request)
        {
            var _res = new BaseRs<userRs>();
            try
            {
                _request.data.password = "123456";
                _request.data.password = _request.data.password.createHash();

                await _userRepository.Create(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<userRs>> Update(BaseRq<userRq> _request)
        {
            var _res = new BaseRs<userRs>();
            try
            {
                await _userRepository.Update(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<userRs>> FindById(int id)
        {
            var _res = new BaseRs<userRs>();
            try
            {
                _res.content = _mapper.Map<userRs>((await _userRepository.FindById(id)));
                _res.content.password = null;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<bool>> Delete(int id)
        {
            var _res = new BaseRs<bool>();
            try
            {
                await _userRepository.Delete(await _userRepository.FindById(id));
                _res.content = true;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}