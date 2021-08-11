using AutoMapper;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.dto.provider;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.provider;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class ProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly Validators.ProviderValidator _validator;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;
        private readonly ICustomCache _cache;

        public ProviderService(Validators.ProviderValidator validator,
                              IProviderRepository companyRepository,
                              EmailService emailService,
                              ICustomCache cache,
                              IMapper mapper)
        {
            _providerRepository = companyRepository;
            _emailService = emailService;
            _validator = validator;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<BaseRs<providerRs>> Show(BaseRq<providerRq> _request)
        {
            var _res = new BaseRs<providerRs>() { content = new providerRs() };
            try
            {
                _res.content.provider = (await _providerRepository.Show(_request.pagination, _request.search))
                    .ConvertAll(s => new providerDto()
                    {
                        ds_situation = this.getSituations().First(f => f.value == ((int)s.situation).ToString()).label,
                        fantasy_name = s.fantasy_name,
                        company_name = s.company_name,
                        situation = s.situation,
                        email = s.email,
                        cnpj = s.cnpj,
                        cpf = s.cpf,
                        id = s.id
                    });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<providerRs>> Store(BaseRq<providerRq> _request)
        {
            var _res = new BaseRs<providerRs>();
            try
            {
                #region ..: pré validations :..

                if (!_request.data.cpf.IsEmpty())
                    if (!_request.data.cpf.IsCpf())
                        _res.error = new BaseError(new List<string> { "CPF informado não e válido." });

                if (!_request.data.cnpj.IsEmpty())
                    if (!_request.data.cnpj.IsCnpj())
                        _res.error = new BaseError(new List<string> { "CNPJ informado não e válido." });

                if (_res.error == null)
                    _res.error = _validator.Check(_request);

                #endregion

                if (_res.error == null)
                {
                    #region ..: check already exists :..

                    _request.data.email = _request.data.email.IsCompare();
                    var company = await _providerRepository.FindByEmail(_request.data.email);
                    if (company != null)
                    {
                        _res.setError("e-mail já cadastrado anteriormente, tente a opção recuperar 'minha senha'");
                        return _res;
                    }

                    company = await _providerRepository.FindByCnpj(_request.data.cnpj);
                    if (company != null)
                    {
                        _res.setError("CNPJ já cadastrado anteriormente, tente a opção recuperar 'minha senha'");
                        return _res;
                    }
                    #endregion

                    var entity = _mapper.Map<Domain.Entities.Provider>(_request.data);
                    entity.situation = Enumerados.ProviderStatus.pending;
                    entity.password = entity.password.createHash();
                    entity.remove = false;

                    await _providerRepository.Create(entity);
                    _emailService.sendWelcome(_request.data);

                    // retorno
                    _res = await this.FindById(entity.id);
                    _res.content.provider[0].password = null;
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<providerRs>> Update(BaseRq<providerRq> _request)
        {
            var _res = new BaseRs<providerRs>();
            try
            {
                #region ..: pré validations :..

                _request.data.phone = _request.data.phone.clearMask().Replace(" ", "");
                _request.data.email = _request.data.email.IsCompare();
                _request.data.cnpj = _request.data.cnpj.clearMask();
                _request.data.cpf = _request.data.cpf.clearMask();

                if (!_request.data.cpf.IsEmpty())
                    if (!_request.data.cpf.IsCpf())
                        _res.error = new BaseError(new List<string> { "CPF informado não e válido." });

                if (!_request.data.cnpj.IsEmpty())
                    if (!_request.data.cnpj.IsCnpj())
                        _res.error = new BaseError(new List<string> { "CNPJ informado não e válido." });

                if (_res.error == null)
                    _res.error = _validator.Check(_request);
                #endregion

                if (_res.error == null)
                {
                    #region ..: endereço :..
                    if (!_request.data.address.IsEmpty())
                    {
                        // address
                        _request.data.address.ForEach(fe => { fe.zipcode = fe.zipcode.clearMask(); });

                        // não enviar para repositorio endereços incompletos
                        if (_request.data.address.Any(a => a.zipcode.IsEmpty()))
                        {
                            _request.data.address = _request.data.address.Where(w => !w.zipcode.IsEmpty()).ToList();
                            if (!_request.data.address.Any())
                                _request.data.address = null;
                        }
                    }
                    #endregion

                    #region ..: dados bancarios :..
                    if (!_request.data.bankAccounts.IsEmpty())
                    {
                        if (_request.data.bankAccounts.Any(a => a.bank_code.IsEmpty()))
                        {
                            _request.data.bankAccounts = _request.data.bankAccounts.Where(w => !w.bank_code.IsEmpty()).ToList();
                            if (!_request.data.bankAccounts.Any())
                                _request.data.bankAccounts = null;
                        }
                    }
                    #endregion

                    // dados
                    var entity = _mapper.Map<Domain.Entities.Provider>(_request.data);
                    await _providerRepository.Update(entity);
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<providerRs>> FindById(int id)
        {
            var _res = new BaseRs<providerRs>() { content = new providerRs() };
            try
            {
                var dto = _mapper.Map<providerDto>(await _providerRepository.FindById(id));
                dto.ds_situation = this.getSituations().First(f => f.value == ((int)dto.situation).ToString()).label;

                if (!dto.languages.IsEmpty()) dto.languages.ForEach(fe => { fe.Provider = null; });
                if (!dto.topics.IsEmpty()) dto.topics.ForEach(fe => { fe.Provider = null; });

                //banks
                var _banks = await _cache.GetBanks();
                dto.bankAccounts.ForEach(fe =>
                {
                    if (_banks.Any(a => a.code == fe.bank_code))
                        fe.ds_bank = _banks.First(f => f.code == fe.bank_code).name;
                });
                _res.content.provider.Add(dto);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public List<Domain.Models.dto.Item> getSituations()
        {
            return new List<Domain.Models.dto.Item>()
            {
                new Domain.Models.dto.Item() { label = "Aprovado", value = ((int)Enumerados.ProviderStatus.approved).ToString() },
                new Domain.Models.dto.Item() { label = "Bloqueado", value = ((int)Enumerados.ProviderStatus.blocked).ToString() },
                new Domain.Models.dto.Item() { label = "Pendente", value = ((int)Enumerados.ProviderStatus.pending).ToString() },
                //new Domain.Models.dto.Item() { label = "Outros", value = ((int)Enumerados.ProviderStatus.others).ToString() },
            };
        }

    }
}