﻿using AutoMapper;
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
    public class ProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly Validators.ProviderValidator _validator;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly ICustomCache _cache;
        private readonly IMapper _mapper;

        public ProviderService(Validators.ProviderValidator validator,
                              IProviderRepository companyRepository,
                              IConfiguration configuration,
                              EmailService emailService,
                              ICustomCache cache,
                              IMapper mapper)
        {
            _providerRepository = companyRepository;
            _configuration = configuration;
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
                        split = !s.SplitAccounts.IsEmpty(),
                        fantasy_name = s.fantasy_name,
                        company_name = s.company_name,
                        completed = s.completed,
                        active = s.active,
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
                    if (_request.data.cnpj.IsNotEmpty())
                    {
                        var company = await _providerRepository.FindByCnpj(_request.data.cnpj);
                        if (company != null)
                        {
                            _res.setError("CNPJ já cadastrado anteriormente, tente a opção recuperar 'minha senha'");
                            return _res;
                        }
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

                _request.data.phone = _request.data.phone.clearMask();
                _request.data.email = _request.data.email.IsCompare();
                _request.data.cnpj = _request.data.cnpj.clearMask();
                _request.data.cpf = _request.data.cpf.clearMask();
                _request.data.youtube = _request.data.youtube;

                if (!_request.data.cpf.IsEmpty())
                    if (!_request.data.cpf.IsCpf())
                        _res.error = new BaseError(new List<string> { "CPF informado não e válido." });

                if (!_request.data.cnpj.IsEmpty())
                    if (!_request.data.cnpj.IsCnpj())
                        _res.error = new BaseError(new List<string> { "CNPJ informado não e válido." });

                if (!_request.data.receipts.IsEmpty())
                {
                    _request.data.receipts.ForEach(fe =>
                    {
                        fe.cpf = fe.cpf.clearMask();
                        if (!fe.cpf.IsEmpty())
                            if (!fe.cpf.IsCpf())
                                _res.error = new BaseError(new List<string> { "CPF informado na assinatura não e válido." });

                        fe.cnpj = fe.cnpj.clearMask();
                        if (!fe.cnpj.IsEmpty())
                            if (!fe.cnpj.IsCnpj())
                                _res.error = new BaseError(new List<string> { "CNPJ informado na assinatura não e válido." });
                    });
                }


                if (_res.error == null)
                    _res.error = _validator.Check(_request);
                #endregion

                if (_res.error == null)
                {
                    #region ..: endereço :..
                    if (!_request.data.address.IsEmpty())
                    {
                        // address
                        _request.data.address.ForEach(fe =>
                        {
                            fe.zipcode = fe.zipcode.clearMask();
                            if (fe.complement.IsNotEmpty() && fe.complement.Length > 100)
                                throw new ArgumentException("Complemento do endereço máximo de 100 caracteres.");
                        });

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
                        _request.data.bankAccounts.ForEach(fe => { if (fe.bank_code.IsCompare() != "104") fe.operation = ""; });
                        if (_request.data.bankAccounts.Any(a => a.bank_code.IsEmpty()))
                        {
                            _request.data.bankAccounts = _request.data.bankAccounts.Where(w => !w.bank_code.IsEmpty()).ToList();
                            if (!_request.data.bankAccounts.Any())
                                _request.data.bankAccounts = null;
                        }
                    }
                    #endregion

                    #region ..: assinatura :..

                    if (!_request.data.receipts.IsEmpty())
                    {
                        if (_request.data.receipts.Any(a => a.signature.IsEmpty()))
                        {
                            _request.data.receipts = _request.data.receipts.Where(w => !w.signature.IsEmpty()).ToList();
                            if (!_request.data.receipts.Any())
                                _request.data.receipts = null;
                        }
                    }
                    #endregion

                    #region ..: validar splitAcoount :..

                    if (_request.data.completed)
                    {
                        if (_request.data.splitAccounts.IsEmpty())
                            throw new ArgumentException("Não e possível completar cadastro, Sem o split de pagamento (NEXXERRA)");
                    }
                    #endregion

                    // dados
                    var entity = _mapper.Map<Domain.Entities.Provider>(_request.data);
                    await _providerRepository.Update(entity);

                    // atualizar cache
                    _cache.Clear("providers");

                    // email de boas vindas, cadastro completo.
                    if (entity.emailWelcomeCompleted)
                    {
                        string msg = $"Parabéns!!! Seu Cadastro está aprovado por nossa equipe. <br>" +
                            $"Você já pode receber atendimentos em nossa plataforma online.";
                        _emailService.sendDefault(entity.email, "Cadastro Aprovado - Clique terapia.", entity.fantasy_name, msg);
                    }
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
                var _provide = await _providerRepository.FindById(id);

                var dto = _mapper.Map<providerDto>(_provide);
                if (this.getSituations().Any(f => f.value == ((int)dto.situation).ToString()))
                    dto.ds_situation = this.getSituations().First(f => f.value == ((int)dto.situation).ToString()).label;

                dto.imageurl = dto.image.toImageUrl($"{_configuration["storage:image"]}/profile");
                dto.price = _provide.price;

                if (dto.receipts.Any(a => a.signature.IsNotEmpty()))
                    dto.signatureurl = dto.receipts[0].signature.toImageUrl($"{_configuration["storage:image"]}/signature");

                if (!dto.languages.IsEmpty()) dto.languages.ForEach(fe => { fe.Provider = null; });
                if (!dto.topics.IsEmpty()) dto.topics.ForEach(fe => { fe.Provider = null; });

                //banks
                var _banks = await _cache.GetBanks();
                dto.bankAccounts.ForEach(fe =>
                {
                    if (_banks.Any(a => a.code == fe.bank_code))
                        fe.ds_bank = _banks.First(f => f.code == fe.bank_code).name;
                });

                // status
                if (!dto.completed)
                {
                    dto.statusCompleted = new providerCompleted();
                    dto.statusCompleted.warnings = new List<Domain.Models.dto.Item>();

                    if (dto.crp.IsEmpty())
                        dto.statusCompleted.warnings.Add(new Domain.Models.dto.Item()
                        {
                            label = "Informe o seu CRP",
                            value = "Dados Pessoais >> CRP",
                            step = 0,
                            code = "crp"
                        });

                    if (dto.address.IsEmpty())
                        dto.statusCompleted.warnings.Add(new Domain.Models.dto.Item()
                        {
                            label = "Informe o seu endereço",
                            value = "Dados Pessoais >> Endereço",
                            step = 0,
                            code = "end"
                        });

                    if (dto.image.IsEmpty())
                        dto.statusCompleted.warnings.Add(new Domain.Models.dto.Item()
                        {
                            label = "Informe uma Imagem para seu perfil",
                            value = "Dados Profissionais >> Alterar Imagem",
                            step = 1,
                            code = "img"
                        });

                    if (dto.description.IsEmpty())
                        dto.statusCompleted.warnings.Add(new Domain.Models.dto.Item()
                        {
                            label = "Informe um Resumo sobre você",
                            value = "Dados Profissionais >> RESUMO SOBRE VOCÊ",
                            step = 1,
                            code = "desc"
                        });

                    if (dto.biography.IsEmpty())
                        dto.statusCompleted.warnings.Add(new Domain.Models.dto.Item()
                        {
                            label = "Informe uma Biografia",
                            value = "Dados Profissionais >> BIOGRAFIA",
                            step = 1,
                            code = "desc"
                        });

                    if (dto.signatureurl.IsEmpty())
                        dto.statusCompleted.warnings.Add(new Domain.Models.dto.Item()
                        {
                            label = "Coloque uma assinatura para emitir recibos",
                            value = "Dados Pagamento >> Faturamento >> Enviar Assinatura",
                            step = 2,
                            code = "ass"
                        });

                    if (dto.bankAccounts.IsEmpty())
                        dto.statusCompleted.warnings.Add(new Domain.Models.dto.Item()
                        {
                            label = "Informe os Dados Bancários para receber seus pagamentos.",
                            value = "Dados Pagamento >> Dados Bancários",
                            step = 2,
                            code = "bnk"
                        });

                    if (_provide.Schedules.IsEmpty())
                        dto.statusCompleted.warnings.Add(new Domain.Models.dto.Item() { label = "Cadastro sem horários disponiveis, cadastre faixa de horários", value = "Meus Horarios >> Faixa de horários", code = "fh" });

                    // percentage.
                    dto.statusCompleted.qtdItens = 7; // 7 - quantidade de ifs
                    if (dto.statusCompleted.warnings.Count > 0)
                    {
                        decimal aa = dto.statusCompleted.warnings.Count();
                        var ss = 100 - ((aa / dto.statusCompleted.qtdItens) * 100);

                        // percent
                        dto.statusCompleted.percent = (int)ss;
                        if (dto.statusCompleted.percent < 0)
                            dto.statusCompleted.percent = 0;
                    }
                    else
                        dto.statusCompleted.percent = 100;
                }
                else
                {
                    if (!dto.active)
                        dto.statusCompleted = new providerCompleted()
                        {
                            warnings = new List<Domain.Models.dto.Item>()
                            {
                                new Domain.Models.dto.Item() { label = "Não disponivel para consultas (INATIVO).", value = "Dados Pessoais >> Estou Disponivel para consultas ?" }
                            }
                        };
                }
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