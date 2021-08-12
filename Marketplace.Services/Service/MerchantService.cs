using AutoMapper;
using Marketplace.Domain.Interface.Integrations.Merchant;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.provider;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class MerchantService
    {
        private readonly ProviderService _providerService;
        private readonly IMerchant _merchant;
        private readonly IMapper _mapper;

        public MerchantService(
            ProviderService providerService,
            IMerchant merchant,
            IMapper mapper)
        {
            _providerService = providerService;
            _merchant = merchant;
            _mapper = mapper;
        }

        public async Task<BaseRs<providerRs>> Store(BaseRq<providerRq> _request)
        {
            var _res = new BaseRs<providerRs>();
            try
            {
                // buscar comerciante
                var entities = await _providerService.FindById(_request.data.id.Value);
                var provider = entities.content.provider[0];

                // validar se existe merchant criado.
                if (provider.splitAccounts.Any(a => a.payment_provider == Domain.Helpers.Enumerados.PaymentProvider.nexxera))
                {
                    _res.setError("Já existe um estabelecimento criado para esse provedor.");
                    return _res;
                }

                // criar estabelecimento
                provider = await _merchant.Create(provider, Domain.Helpers.Enumerados.PaymentProvider.nexxera);

                // mapper
                var mapper = _mapper.Map<providerRq>(provider);

                //atualizar
                await _providerService.Update(new BaseRq<providerRq>() { data = mapper });
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}