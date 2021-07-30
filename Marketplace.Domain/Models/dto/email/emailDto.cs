namespace Marketplace.Domain.Models.dto.email
{
    public class emailDto
    {
        public emailDto()
        {
            this.display = null;
        }

        public string title { get; set; }
        public string display { get; set; }
        public string body { get; set; }
        public string email { get; set; }
    }
}
