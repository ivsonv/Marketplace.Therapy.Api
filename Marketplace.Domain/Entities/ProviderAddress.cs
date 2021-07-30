namespace Marketplace.Domain.Entities
{
    public class ProviderAddress : shared.BaseAddress
    {
        public int provider_id { get; set; }
        public Provider Provider { get; set; }
    }
}
