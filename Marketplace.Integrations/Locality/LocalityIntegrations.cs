using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.dto.location;
using System.Threading.Tasks;

namespace Marketplace.Integrations.Locality
{
    public class LocalityIntegrations : Domain.Interface.Integrations.Locality.ILocality
    {
        public Task<Address> getLocation(string zipCode, Enumerados.LocalityProvider provider)
        {
            switch (provider)
            {
                case Enumerados.LocalityProvider.correios:
                    return Correios.CorreioClient.getAddress(zipCode);

                default:
                    return null;
            }
        }
    }
}
