using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.dto.customer;
using Marketplace.Domain.Models.permissions;
using Marketplace.Domain.Models.Request.auth.customer;
using Marketplace.Domain.Models.Request.auth.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.auth.customer;
using Marketplace.Domain.Models.Response.auth.provider;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class AuthService
    {
        private readonly Validators.CustomerAuthValidator _validator;
        private readonly IGroupPermissionRepository _groupPermissionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public AuthService(Validators.CustomerAuthValidator validator,
                           IGroupPermissionRepository groupPermissionRepository,
                           ICustomerRepository customerRepository,
                           IProviderRepository providerRepository,
                           IUserRepository userRepository,
                           IConfiguration configuration,
                           EmailService emailService)
        {
            _groupPermissionRepository = groupPermissionRepository;
            _providerRepository = providerRepository;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
            _validator = validator;
        }

        public async Task<BaseRs<customerAuthRs>> Customer(customerAuthRq auth)
        {
            var _res = new BaseRs<customerAuthRs>();
            try
            {
                _res.error = _validator.Check(auth);
                if (_res.error == null)
                {
                    var _customer = await _customerRepository.FindAuthByEmail(auth.login.IsCompare());
                    if (_customer == null)
                    {
                        _res.setError("Usuário/senha informado não existe.");
                        return _res;
                    }

                    if (_customer.password != auth.password.createHash())
                    {
                        _res.setError("Usuário/Senha informado não existe.");
                        return _res;
                    }

                    // userRuleCustomer
                    var _repo = await _groupPermissionRepository.FindByName("customer");
                    var dto = new Domain.Models.dto.auth.AuthDto()
                    {
                        id = _customer.id,
                        name = _customer.name,
                        roles = _repo.PermissionsAttached.Select(s => s.name), // nome da permissão
                        permissions = new List<int>() { _repo.id }             // id do grupo de permissão, usado no customPermission
                    };

                    // content
                    _res.content = new customerAuthRs()
                    {
                        accessToken = CustomExtensions.GenerateToken(dto, _configuration["secrets:signingkey"]),
                        data = new Domain.Models.Response.auth.AuthData()
                        {
                            fullName = dto.name,
                            roles = dto.roles,
                            avatar = null,
                            id = dto.id
                        }
                    };
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<bool>> CustomerResetPassword(customerAuthRq auth)
        {
            var _res = new BaseRs<bool>();
            try
            {
                if (auth.login.IsNotEmpty() && auth.login.IsEmail())
                {
                    var _customer = await _customerRepository.FindByEmail(auth.login);
                    if (_customer != null)
                    {
                        //Gerar novo
                        _customer.recoverpassword = $"{CustomExtensions.getGuid}";
                        await _customerRepository.Update(_customer);

                        // send email
                        _emailService.sendResetPassword(new customerDto()
                        {
                            email = _customer.email,
                            name = _customer.name
                        }, _customer.recoverpassword);
                        _res.content = true;
                    }
                }
                else
                {
                    _res.setError("Solicitação enviada não e valida.");
                    return _res;
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<bool>> CustomerUpdatePassword(customerAuthRq auth)
        {
            var _res = new BaseRs<bool>();
            try
            {
                if (auth.token.IsEmpty())
                {
                    _res.setError("Solicitação enviada não e valida");
                    return _res;
                }

                var _customer = await _customerRepository.FindByToken(auth.token);
                if (_customer != null)
                {
                    // alterar
                    _customer.password = auth.password.createHash();
                    _customer.recoverpassword = null;
                    await _customerRepository.Update(_customer);
                    _res.content = true;
                }
                else
                {
                    _res.setError("Solicitação não pode ser processada.");
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<customerAuthRs>> Provider(customerAuthRq auth)
        {
            var _res = new BaseRs<customerAuthRs>();
            try
            {
                _res.error = _validator.Check(auth);
                if (_res.error == null)
                {
                    var _provider = await _providerRepository.FindAuthByEmail(auth.login.IsCompare());
                    if (_provider == null)
                    {
                        _res.setError("Usuário/senha informado não existe.");
                        return _res;
                    }

                    if (_provider.password != auth.password.createHash())
                    {
                        _res.setError("Usuário/Senha informado não existe.");
                        return _res;
                    }

                    var _repo = await _groupPermissionRepository.FindByName("provider");
                    var dto = new Domain.Models.dto.auth.AuthDto()
                    {
                        id = _provider.id,
                        name = _provider.nickname.IsNotEmpty() ? _provider.nickname : _provider.fantasy_name,
                        roles = _repo.PermissionsAttached.Select(s => s.name), // nome da permissão
                        permissions = new List<int>() { _repo.id }             // id do grupo de permissão, usado no customPermission
                    };

                    // content
                    _res.content = new customerAuthRs()
                    {
                        accessToken = CustomExtensions.GenerateToken(dto, _configuration["secrets:signingkey"]),
                        data = new Domain.Models.Response.auth.AuthData()
                        {
                            avatar = _provider.image.IsNotEmpty()
                            ? _provider.image.toImageUrl($"{_configuration["storage:image"]}/profile")
                            : null,
                            fullName = dto.name,
                            roles = dto.roles,
                            id = dto.id
                        }
                    };
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<bool>> ProviderResetPassword(providerAuthRq auth)
        {
            var _res = new BaseRs<bool>();
            try
            {
                if (auth.login.IsNotEmpty() && auth.login.IsEmail())
                {
                    var _provider = await _providerRepository.FindByEmail(auth.login);
                    if (_provider != null)
                    {
                        //Gerar novo
                        _provider.recoverpassword = $"{CustomExtensions.getGuid}";
                        await _providerRepository.UpdateRecover(_provider);

                        // send email
                        _emailService.sendResetPassword(new customerDto()
                        {
                            email = _provider.email,
                            name = $"{_provider.fantasy_name} {_provider.company_name}"
                        }, _provider.recoverpassword);

                        //
                        _res.content = true;
                    }
                }
                else
                {
                    _res.setError("Solicitação enviada não e valida.");
                    return _res;
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
        public async Task<BaseRs<bool>> ProviderUpdatePassword(providerAuthRq auth)
        {
            var _res = new BaseRs<bool>();
            try
            {
                if (_res.error == null)
                {
                    if (auth.token.IsEmpty())
                    {
                        _res.setError("Solicitação enviada não e valida");
                        return _res;
                    }

                    var _provider = await _providerRepository.FindByToken(auth.token);
                    if (_provider != null)
                    {
                        // alterar
                        _provider.password = auth.password.createHash();
                        _provider.recoverpassword = null;
                        await _providerRepository.UpdateRecover(_provider);
                        _res.content = true;
                    }
                    else
                    {
                        _res.setError("Solicitação não pode ser processada.");
                    }
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        //ADMIN
        public async Task<BaseRs<providerAuthRs>> Admin(providerAuthRq auth)
        {
            var _res = new BaseRs<providerAuthRs>();
            try
            {
                var _user = await _userRepository.FindAuthByEmail(auth.login.IsCompare());
                if (_user == null)
                {
                    _res.setError("Usuário/senha informado não existe.");
                    return _res;
                }

                if (!_user.active)
                {
                    _res.setError("Usuário inativo.");
                    return _res;
                }

                if (_user.password != auth.password.createHash())
                {
                    _res.setError("Usuário/Senha informado não existe.");
                    return _res;
                }

                if (_user.GroupPermissions.IsEmpty())
                {
                    _res.setError("Usuário/senha sem grupo de permissão.");
                    return _res;
                }

                // permissões
                var _permissions = _user.GroupPermissions.SelectMany(ss => ss.GroupPermission.PermissionsAttached)
                                                         .Select(s => s.name).Distinct().ToList();

                // dto token
                var dto = new Domain.Models.dto.auth.AuthDto()
                {
                    permissions = _user.GroupPermissions.Select(s => s.group_permission_id),
                    name = _user.name,
                    id = _user.id,
                    roles = null
                };

                // retorno
                _res.content = new providerAuthRs()
                {
                    accessToken = CustomExtensions.GenerateToken(dto, _configuration["secrets:signingkey"]),
                    data = new Domain.Models.Response.auth.AuthData()
                    {
                        roles = _permissions,
                        fullName = dto.name,
                        id = dto.id
                    }
                };
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}