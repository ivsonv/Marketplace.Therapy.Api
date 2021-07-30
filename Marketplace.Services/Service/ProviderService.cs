using AutoMapper;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.dto.company;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.company;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.company;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class ProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly Validators.CompanyValidator _validator;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;

        public ProviderService(Validators.CompanyValidator validator,
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
                _res.content.company = (await _providerRepository.Show(_request.pagination))
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
                    _res.content.company[0].password = null;
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
                _res.content.company.Add(_mapper.Map<providerDto>(entity));
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        //public async Task<BaseRs<customerRs>> Update(BaseRq<customerRq> _request)
        //{
        //    var _res = new BaseRs<customerRs>();
        //    try
        //    {
        //        _res.error = _validator.Check(_request);
        //        if (_res.error == null)
        //        {
        //            if (!_request.data.id.HasValue)
        //            {
        //                _res.setError("Id e obrigatório");
        //                return _res;
        //            }
        //            _request.data.email = _request.data.email.IsCompare(); // padronizar email

        //            // Ler Registro
        //            var entity = await _customerRepository.FindById(_request.data.id.Value);
        //            _request.data.password = entity.password; // manter password, existe outra função para mudar e-mail

        //            if (entity.email != _request.data.email)
        //            {
        //                // email diferente do atual, validar se já existe
        //                #region ..: check email already exists :..

        //                _request.data.email = _request.data.email.IsCompare();
        //                var user = _customerRepository.FindByEmail(_request.data.email);
        //                if (user != null)
        //                {
        //                    _res.setError("Não e possível atualizar cliente, e-mail já cadastrado.");
        //                    return _res;
        //                }
        //                #endregion
        //            }

        //            // mapear request para entidade
        //            entity = _mapper.Map<Domain.Entities.Customer>(_request.data);

        //            // atualizar
        //            await _customerRepository.Update(entity);
        //            _res = await this.FindById(entity.id);
        //        }
        //    }
        //    catch (System.Exception ex) { _res.setError(ex); }
        //    return _res;
        //}

        //public async Task<BaseRs<customerRs>> FindById(int id)
        //{
        //    var _res = new BaseRs<customerRs>() { content = new customerRs() };
        //    try
        //    {
        //        var entity = await _customerRepository.FindById(id);
        //        _res.content.customer.Add(_mapper.Map<customerDto>(entity));
        //    }
        //    catch (System.Exception ex) { _res.setError(ex); }
        //    return _res;
        //}

        //public async Task<BaseRs<bool>> Delete(int id)
        //{
        //    var _res = new BaseRs<bool>();
        //    try
        //    {
        //        await _customerRepository.Delete(await _customerRepository.FindById(id));
        //        _res.content = true;
        //    }
        //    catch (System.Exception ex) { _res.setError(ex); }
        //    return _res;
        //}
    }
}