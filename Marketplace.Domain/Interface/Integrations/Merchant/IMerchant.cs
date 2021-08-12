using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Integrations.Merchant
{
    public interface IMerchant
    {
        Task<Models.dto.provider.providerDto> Create(Models.dto.provider.providerDto dto, Helpers.Enumerados.PaymentProvider provider);
    }
}