using Marketplace.Domain.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Integrations.Locality.Correios
{
    public static class CorreioClient
    {
        public static async Task<Domain.Models.dto.location.Address> getAddress(string zipcode)
        {
            zipcode = string.Join("", zipcode.ToCharArray().Where(Char.IsDigit));
            if(!zipcode.IsEmpty())
            {
                var _client = new ServiceReference1.AtendeClienteClient();
                var res = await _client.consultaCEPAsync(zipcode);
                if(res != null && res.@return != null)
                {
                    return new Domain.Models.dto.location.Address()
                    {
                        uf = res.@return.uf.ToUpper().Trim(),
                        city = res.@return.cidade.RemoveAccents().Trim(),
                        neighborhood = res.@return.bairro.RemoveAccents().Trim(),
                        zipcode = res.@return.cep.Trim(),
                        address = res.@return.end.RemoveAccents().Trim(),
                        country = "BR"
                    };
                }
            }
            return null;
        }

    }
}
