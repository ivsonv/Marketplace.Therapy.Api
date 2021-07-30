using Marketplace.Domain.Interface.Integrations.Locality;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.locations;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class LocationService
    {
        private readonly ILocality _ILocalition;

        public LocationService(ILocality ILocalition)
        {
            _ILocalition = ILocalition;
        }

        public async Task<BaseRs<locationRs>> FindByZipCode(string zipcode)
        {
            var _res = new BaseRs<locationRs>();
            try
            {
                _res.content = new locationRs();
                _res.content.address = await _ILocalition.getLocation(zipcode, Domain.Helpers.Enumerados.LocalityProvider.correios);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            return _res;
        }
    }
}