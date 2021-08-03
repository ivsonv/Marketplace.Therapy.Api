using AutoMapper;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.dto.provider;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.provider;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class ProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly Validators.ProviderValidator _validator;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;

        public ProviderService(Validators.ProviderValidator validator,
                              IProviderRepository companyRepository,
                              EmailService emailService,
                              IMapper mapper)
        {
            _providerRepository = companyRepository;
            _emailService = emailService;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<BaseRs<providerRs>> Show(BaseRq<providerRq> _request)
        {
            var _res = new BaseRs<providerRs>() { content = new providerRs() };
            try
            {
                _res.content.provider = (await _providerRepository.Show(_request.pagination))
                    .ConvertAll(s => new providerDto()
                    {
                        fantasy_name = s.fantasy_name,
                        company_name = s.company_name,
                        email = s.email,
                        cnpj = s.cnpj,
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
                _res.error = _validator.Check(_request);
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

        public async Task<BaseRs<providerRs>> FindById(int id)
        {
            var _res = new BaseRs<providerRs>() { content = new providerRs() };
            try
            {
                var entity = await _providerRepository.FindById(id);
                _res.content.provider.Add(_mapper.Map<providerDto>(entity));
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

    }
}