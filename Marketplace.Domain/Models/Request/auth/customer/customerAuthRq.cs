namespace Marketplace.Domain.Models.Request.auth.customer
{
    public class customerAuthRq
    {
        public string login { get; set; }
        public string password { get; set; }
        public string token  { get; set; }
    }
}
