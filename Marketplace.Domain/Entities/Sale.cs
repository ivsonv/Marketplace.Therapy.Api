namespace Marketplace.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public int customer_id { get; set; }
        public int provider_id { get; set; }        
        public int price_full { get; set; }
        public int price { get; set; }
        public Helpers.Enumerados.PaymentStatus payment_status { get; set; }
        public Helpers.Enumerados.SalesStatus sale_status { get; set; }
    }
}