namespace Marketplace.Domain.Models.Request.topics
{
    public class topicRq
    {
        public string name { get; set; }
        public int id { get; set; }
        public bool experience { get; set; }
        public bool? active { get; set; }
    }
}
