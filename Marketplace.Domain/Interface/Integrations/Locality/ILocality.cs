using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Integrations.Locality
{
    public interface ILocality
    {
        Task<Models.dto.location.Address> getLocation(string zipCode, Helpers.Enumerados.LocalityProvider provider);
    }
}
