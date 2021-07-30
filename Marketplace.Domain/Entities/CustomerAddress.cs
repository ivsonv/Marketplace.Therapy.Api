namespace Marketplace.Domain.Entities
{
    public class CustomerAddress : shared.BaseAddress
    {
        public int customer_id { get; set; }
        public Customer Customer { get; set; }
    }
}