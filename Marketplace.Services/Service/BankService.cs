using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.banks;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.banks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class BankService
    {
        private readonly IBankRepository _bankRepository;

        public BankService(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<BaseRs<List<bankRs>>> show(BaseRq<bankRq> _request)
        {
            var _res = new BaseRs<List<bankRs>>();
            try
            {
                var lst = await _bankRepository.Show(_request.pagination, _request.search);
                _res.content = lst.ConvertAll(cc => new bankRs() { id = cc.id, name = cc.name, active = cc.active, code = cc.code });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<bankRs>> Store(BaseRq<bankRq> _request)
        {
            var _res = new BaseRs<bankRs>();
            try
            {
                await _bankRepository.Create(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<bankRs>> Update(BaseRq<bankRq> _request)
        {
            var _res = new BaseRs<bankRs>();
            try
            {
                await _bankRepository.Update(_request.data);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public async Task<BaseRs<bankRs>> FindById(int id)
        {
            var _res = new BaseRs<bankRs>();
            try
            {
                var entity = await _bankRepository.FindById(id);
                if (entity != null)
                {
                    _res.content = new bankRs()
                    {
                        active = entity.active,
                        code = entity.code,
                        name = entity.name,
                        id = entity.id
                    };
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
                await _bankRepository.Delete(await _bankRepository.FindById(id));
                _res.content = true;
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }

        public List<Domain.Models.dto.Item> getAccountTypes()
        {
            return new List<Domain.Models.dto.Item>()
            {
                new Domain.Models.dto.Item() { label = "Conta Corrente", value = ((int)Enumerados.AccountBankType.contaCorrente).ToString() },
                new Domain.Models.dto.Item() { label = "Conta Poupança", value = ((int)Enumerados.AccountBankType.contaPoupanca).ToString() },
                //new Domain.Models.dto.Item() { label = "Conta Pagamento", value = ((int)Enumerados.AccountBankType.contaPagamento).ToString() },
            };
        }
    }
}