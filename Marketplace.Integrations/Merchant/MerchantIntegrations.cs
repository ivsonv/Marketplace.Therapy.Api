using Marketplace.Domain.Models.dto.payment;
using Marketplace.Domain.Models.dto.provider;
using System.Threading.Tasks;

namespace Marketplace.Integrations.Payment
{
    public class MerchantIntegrations : Domain.Interface.Integrations.Merchant.IMerchant
    {
        public Task<providerDto> Create(providerDto dto, Domain.Helpers.Enumerados.PaymentProvider pp)
        {
            switch (pp)
            {
                case Domain.Helpers.Enumerados.PaymentProvider.nexxera:
                    Nexxera.NexxeraClient.CreateMerchant(dto); break;
                default:
                    throw new System.NotImplementedException();
            }
            return Task.FromResult(dto);
        }
    }
}