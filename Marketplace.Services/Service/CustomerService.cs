﻿using AutoMapper;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.dto.customer;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.customers;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.customers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class CustomerService
    {
        private readonly Validators.CustomerValidator _validator;
        private readonly EmailService _emailService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(Validators.CustomerValidator validator,
                               ICustomerRepository customerRepository,
                               EmailService emailService,
                               IMapper mapper)
        {
            _customerRepository = customerRepository;
            _emailService = emailService;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<BaseRs<customerRs>> show(BaseRq<customerRq> _request)
        {
            var _res = new BaseRs<customerRs>() { content = new customerRs() };
            try
            {
                var lst = await _customerRepository.Show(_request.pagination, _request.search);
                _res.content.customer = _mapper.Map<List<customerDto>>(lst);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<customerRs>> Store(BaseRq<customerRq> _request)
        {
            var _res = new BaseRs<customerRs>();
            try
            {
                _res.error = _validator.Check(_request);
                if (_res.error == null)
                {
                    #region ..: check email already exists :..

                    _request.data.email = _request.data.email.IsCompare();
                    var user = await _customerRepository.FindByEmail(_request.data.email);
                    if (user != null)
                    {
                        _res.setError("e-mail já cadastrado, tente a opção recuperar 'minha senha'");
                        return _res;
                    }
                    #endregion

                    #region ..: check cpf already exists :..

                    _request.data.cpf = _request.data.cpf.clearMask();
                    user = await _customerRepository.FindByCpf(_request.data.cpf);
                    if (user != null)
                    {
                        _res.setError("CPF já cadastrado, tente a opção recuperar 'minha senha'");
                        return _res;
                    }
                    #endregion

                    var entity = _mapper.Map<Domain.Entities.Customer>(_request.data);
                    entity.password = entity.password.createHash();
                    entity.active = true;

                    await _customerRepository.Create(entity);

                    string msg = "Bem-vindo ao mundo Clique Terapia <br><br>" +
                        "Somos o maior site de consultas online com psicólogos do Brasil & " +
                        "os mais recomendados por todos que amam esse segmento.";
                    _emailService.sendDefault(entity.email, "Bem-vindo Clique Terapia", entity.name, msg);
                    _res = await this.FindById(entity.id);

                    // clear password
                    if (_res.content != null && !_res.content.customer.IsEmpty())
                        _res.content.customer[0].password = null;
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<customerRs>> Update(BaseRq<customerRq> _request)
        {
            var _res = new BaseRs<customerRs>();
            try
            {
                _res.error = _validator.Check(_request);
                if (_res.error == null)
                {
                    if (!_request.data.id.HasValue || _request.data.id.Value <= 0)
                    {
                        _res.setError("Id e obrigatório");
                        return _res;
                    }
                    _request.data.email = _request.data.email.IsCompare(); // padronizar email

                    // Ler Registro
                    var entity = await _customerRepository.FindById(_request.data.id.Value);
                    _request.data.password = entity.password; // manter password, existe outra função para mudar e-mail

                    if (entity.email != _request.data.email)
                    {
                        // email diferente do atual, validar se já existe
                        #region ..: check email already exists :..

                        _request.data.email = _request.data.email.IsCompare();
                        var user = await _customerRepository.FindByEmail(_request.data.email);
                        if (user != null)
                        {
                            _res.setError("Não e possível atualizar cliente, e-mail já cadastrado.");
                            return _res;
                        }
                        #endregion
                    }

                    if (entity.cpf != _request.data.cpf)
                    {
                        // cpf diferente do atual, validar se já existe
                        #region ..: check cpf already existis :..

                        _request.data.cpf = _request.data.cpf.IsCompare();
                        var user = await _customerRepository.FindByCpf(_request.data.cpf);
                        if (user != null)
                        {
                            _res.setError("Não e possível atualizar, cpf já cadastrado.");
                            return _res;
                        }
                        #endregion
                    }

                    // mapear request para entidade
                    entity = _mapper.Map<Domain.Entities.Customer>(_request.data);

                    // atualizar
                    await _customerRepository.Update(entity);
                    _res = await this.FindById(entity.id);

                    // clear password
                    if (_res.content != null && !_res.content.customer.IsEmpty())
                        _res.content.customer[0].password = null;
                }
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<customerRs>> FindById(int id)
        {
            var _res = new BaseRs<customerRs>() { content = new customerRs() };
            try
            {
                var entity = await _customerRepository.FindById(id);
                _res.content.customer.Add(_mapper.Map<customerDto>(entity));
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<dynamic>> ShowAppointments(int customer_id)
        {
            var _res = new BaseRs<dynamic>();
            try
            {
                _res.content = (await _customerRepository.ShowAppointments(customer_id))
                                .ConvertAll(x => new
                                {
                                    provider = new
                                    {
                                        name = $"{x.Provider.fantasy_name} {x.Provider.company_name}",
                                        id = x.Provider.id
                                    },
                                    payment = new
                                    {
                                        ds = x.payment_status.ToString()
                                    },
                                    booking_date = x.booking_date.ToString("dd/MM/yyyy HH:mm"),
                                    created_at = x.created_at.Value.ToString("dd/MM/yyyy HH:mm"),
                                    price = $"R$ {x.price.ToString("N2")}",
                                    id = x.id
                                });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<bool>> Delete(int id)
        {
            var _res = new BaseRs<bool>();
            try
            {
                await _customerRepository.Delete(await _customerRepository.FindById(id));
                _res.content = true;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}