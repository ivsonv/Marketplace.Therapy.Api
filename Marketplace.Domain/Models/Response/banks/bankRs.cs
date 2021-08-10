namespace Marketplace.Domain.Models.Response.banks
{
    public class bankRs : Entities.Bank
    {
        public string name_format
        {
            get { return $"{base.code} - {base.name}"; }
        }
    }
}