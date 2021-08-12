using Marketplace.Domain.Entities;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models;
using Microsoft.EntityFrameworkCore;
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

            var query = _repository.Get(order: o => o.id, pagination);

            // filter name
            if (name.IsNotEmpty())
                query = query.Where(w => w.fantasy_name != null && w.fantasy_name.ToLower().Contains(name) ||
                                         w.company_name != null && w.company_name.ToLower().Contains(name) ||
                                         w.nickname != null && w.nickname.ToLower().Contains(name) ||
                                         w.email != null && w.email.ToLower().Contains(name) ||
                                         w.cnpj != null && w.cnpj.ToLower().Contains(name) ||
                                         w.cpf != null && w.cpf.ToLower().Contains(name));
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
            }).ToListAsync();
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
                                    .FirstOrDefaultAsync(f => f.id == id);
        }

        public async Task Create(Provider entity)
        {
            this.formatData(entity);

            _repository.Add(entity);
            await _repository.SaveChanges();
        }
        private void formatData(Provider entity)
        {
            if (!entity.fantasy_name.IsEmpty()) entity.fantasy_name = entity.fantasy_name.Clear().ToUpper();
            if (!entity.company_name.IsEmpty()) entity.company_name = entity.company_name.Clear().ToUpper();
            if (!entity.nickname.IsEmpty()) entity.nickname = entity.nickname.Clear().ToUpper();
        }

        public async Task Update(Provider entity)
        {
            this.formatData(entity);

            var _current = await this.FindById(entity.id);
            if (_current != null)
            {
                _current.academic_training = entity.academic_training;
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
                _current.curriculum = entity.curriculum;

                //endereço
                if (entity.Address != null)
                    _current.Address = entity.Address;

                // dados bancarios
                if (entity.BankAccounts != null)
                    _current.BankAccounts = entity.BankAccounts;

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

                    _current.Topics = null;
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
                      fantasy_name = s.fantasy_name,
                      id = s.id
                  }).FirstOrDefaultAsync();
        }
        public async Task<Provider> FindByEmail(string email)
            => await _repository.Query.FirstOrDefaultAsync(f => f.email == email);
        public async Task<Provider> FindByCnpj(string cnpj)
            => await _repository.Query.FirstOrDefaultAsync(f => f.cnpj == cnpj);

        public Task Delete(List<Provider> entity)
        {
            throw new System.NotImplementedException();
        }
    }
}