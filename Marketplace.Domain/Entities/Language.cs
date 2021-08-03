namespace Marketplace.Domain.Entities
{
    public class Language : BaseEntity
    {
        public string name { get; set; }
        public bool active { get; set; }
    }
}