namespace Marketplace.Domain.Models.dto.payment
{
    public class CardDto
    {
        public double total_price { get; set; }
        public int installment { get; set; }
        public double installment_price { get; set; }        
        public string number { get; set; }
        public string cvv { get; set; }
        public string expiration_day { get; set; }
        public string expiration_year { get; set; }
        public string holder { get; set; }
        public string token { get; set; }
        public Helpers.Enumerados.CardBrand cardbrand { get; set; }
    }
}
