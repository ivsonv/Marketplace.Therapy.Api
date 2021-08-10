namespace Marketplace.Domain.Entities
{
    public class Bank : BaseEntity
    {
        public string name { get; set; }
        public string code { get; set; }
        public bool active { get; set; }
    }
}