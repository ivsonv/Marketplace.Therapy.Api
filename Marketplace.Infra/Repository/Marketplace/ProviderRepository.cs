using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Infra.Repository.Marketplace
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly BaseRepository<ProviderLanguages> _repositoryLanguages;
        private readonly BaseRepository<ProviderTopics> _repositoryTopics;
        private readonly BaseRepository<Provider> _repository;

        public ProviderRepository(BaseRepository<ProviderLanguages> repositoryLanguages,
                                  BaseRepository<ProviderTopics> repositoryTopics,
                                  BaseRepository<Provider> repository)
        {
            _repositoryLanguages = repositoryLanguages;
            _repositoryTopics = repositoryTopics;
            _repository = repository;
        }
        public async Task<List<Provider>> Show(Pagination pagination, string search = "")
        {
            string name = search.Split('|')[0].Replace("null", "").ToLower().Clear();
            int? situation = (search.Split('|')[1] != "-1") ? int.Parse(search.Split('|')[1]) : null;

            var query = _repository.Query.AsQueryable();

            // filter name
            if (name.IsNotEmpty())
                query = query.Where(w => w.fantasy_name.ToLower().Trim().Contains(name) ||
                                         w.company_name.ToLower().Trim().Contains(name) ||
                                         w.email.ToLower().Trim().Contains(name) ||
                                         w.nickname != null && w.nickname.ToLower().Trim().Contains(name) ||
                                         w.cnpj != null && w.cnpj.ToLower().Trim().Contains(name) ||
                                         w.cpf != null && w.cpf.ToLower().Trim().Contains(name));
            // filter situation
            if (situation != null)
                query = query.Where(w => (int)w.situation == situation);

            return await query.Select(s => new Provider()
            {
                fantasy_name = s.fantasy_name,
                company_name = s.company_name,
                situation = s.situation,
                email = s.email,
                cnpj = s.cnpj,
                cpf = s.cpf,
                id = s.id
            }).Skip(pagination.size * pagination.page)
              .Take(pagination.size).OrderBy(o => o.id)
              .ToListAsync();
        }
        public async Task<List<Provider>> Show(Pagination pagination)
        {
            return await this.Show(pagination);
        }

        public async Task<Provider> FindById(int id)
        {
            return await _repository.Query
                                    .Include(i => i.Address)
                                    .Include(i => i.BankAccounts)
                                    .Include(i => i.Languages)
                                    .Include(i => i.Topics)
                                    .Include(i => i.SplitAccounts)
                                    .Include(i => i.Receipts)
                                    .FirstOrDefaultAsync(f => f.id == id);
        }

        private void formatData(Provider entity)
        {
            if (!entity.fantasy_name.IsEmpty()) entity.fantasy_name = entity.fantasy_name.Clear().ToUpper();
            if (!entity.company_name.IsEmpty()) entity.company_name = entity.company_name.Clear().ToUpper();
            if (!entity.nickname.IsEmpty()) entity.nickname = entity.nickname.Clear().ToUpper();
        }
        public async Task Create(Provider entity)
        {
            this.formatData(entity);

            var exist = await this.FindAuthByEmail(entity.email);
            if (exist == null)
            {
                _repository.Add(entity);
                await _repository.SaveChanges();
            }
            else
                throw new ArgumentException("E-mail já cadastrado anteriormente, tente a opção 'esqueci minha senha'");
        }
        public async Task Update(Provider entity)
        {
            this.formatData(entity);

            var _current = await this.FindById(entity.id);
            if (_current != null)
            {
                // mudou e-mail
                if (_current.email != entity.email)
                {
                    if ((await this.FindAuthByEmail(entity.email)) != null)
                        throw new ArgumentException("e-mail já está em uso para outro usuário");
                }

                // mudou cpf
                if (_current.cpf.IsNotEmpty() && _current.cpf != entity.cpf)
                {
                    if ((await this.FindByCpf(entity.cpf)) != null)
                        throw new ArgumentException("cpf já está em uso para outro usuário");
                }

                // mudou cnpj
                if (_current.cnpj.IsNotEmpty() && _current.cnpj != entity.cnpj)
                {
                    if ((await this.FindByCnpj(entity.cnpj)) != null)
                        throw new ArgumentException("cnpj já está em uso para outro usuário");
                }

                // preencher automatico
                if (entity.link.IsEmpty())
                    entity.link = $"{entity.fantasy_name}-{entity.company_name}".IsCompare();

                // mudou link meu site
                if (_current.link.IsNotEmpty() && _current.link != entity.link)
                {
                    if ((await this.FindByLink(entity.link)) != null)
                        throw new ArgumentException("link já está em uso para outro usuário.");
                }

                _current.academic_training = entity.academic_training;
                _current.description = entity.description;
                _current.company_name = entity.company_name;
                _current.fantasy_name = entity.fantasy_name;
                _current.biography = entity.biography;
                _current.situation = entity.situation;
                _current.nickname = entity.nickname;
                _current.active = entity.active;
                _current.phone = entity.phone;
                _current.cnpj = entity.cnpj;
                _current.cpf = entity.cpf;
                _current.crp = entity.crp;
                _current.email = entity.email;
                _current.image = entity.image;
                _current.link = entity.link;
                _current.price = entity.price;

                //endereço
                if (entity.Address != null)
                    _current.Address = entity.Address;

                // dados bancarios
                if (entity.BankAccounts != null)
                    _current.BankAccounts = entity.BankAccounts;

                // dados assinatura
                if (entity.Receipts != null)
                    _current.Receipts = entity.Receipts;

                #region ..: Idiomas :..

                var receives = entity.Languages.Select(s => s.language_id).ToList();
                var lanCurrents = _current.Languages.Select(s => s.language_id).ToList();
                var lanRemoves = lanCurrents.Where(w => !receives.Contains(w)).ToList();
                if (lanRemoves.Any())
                {
                    var lst = _current.Languages.Where(w => lanRemoves.Contains(w.language_id)).ToList();
                    _repositoryLanguages.RemoveRange(lst);

                    _current.Languages = null;
                }

                receives = receives.Where(w => !lanCurrents.Contains(w)).ToList();
                if (receives.Any())
                    _current.Languages = receives.ConvertAll(c
                        => new ProviderLanguages()
                        {
                            language_id = c
                        });
                #endregion

                #region ..: Temas / Topics :..

                receives = entity.Topics.Select(s => s.topic_id).ToList();
                var topicsCurrents = _current.Topics.Select(s => s.topic_id).ToList();
                var topicsRemoves = topicsCurrents.Where(w => !receives.Contains(w)).ToList();
                if (topicsRemoves.Any())
                {
                    var lst = _current.Topics.Where(w => topicsRemoves.Contains(w.topic_id)).ToList();
                    _repositoryTopics.RemoveRange(lst);
                }

                receives = receives.Where(w => !topicsCurrents.Contains(w)).ToList();
                if (receives.Any())
                    _current.Topics = receives.ConvertAll(c
                        => new ProviderTopics()
                        {
                            topic_id = c
                        });
                #endregion
            }

            _repository.Update(_current);
            await _repository.SaveChanges();
        }
        public async Task Delete(Provider entity)
        {
            _repository.Remove(entity);
            await _repository.SaveChanges();
        }

        public async Task<Provider> FindAuthByEmail(string email)
        {
            return await _repository.Get(g => g.email == email)
                  .Select(s => new Provider()
                  {
                      image = s.image,
                      fantasy_name = s.fantasy_name,
                      password = s.password,
                      id = s.id
                  }).FirstOrDefaultAsync();
        }
        public async Task<Provider> FindByEmail(string email)
            => await _repository.Query.FirstOrDefaultAsync(f => f.email == email);
        public async Task<Provider> FindByCnpj(string cnpj)
            => await _repository.Query.FirstOrDefaultAsync(f => f.cnpj == cnpj);
        public async Task<Provider> FindByLink(string link)
            => await _repository.Query.FirstOrDefaultAsync(f => f.link == link);

        public async Task<Provider> FindByCpf(string cpf)
            => await _repository.Query.FirstOrDefaultAsync(f => f.cpf == cpf);
        public Task Delete(List<Provider> entity)
        {
            throw new System.NotImplementedException();
        }
    }
}