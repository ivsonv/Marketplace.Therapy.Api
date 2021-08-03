namespace Marketplace.Domain.Entities
{
    public class Topic : BaseEntity
    {
        public string name { get; set; }
        public bool active { get; set; }
    }
}