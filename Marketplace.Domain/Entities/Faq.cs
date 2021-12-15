using System.Collections.Generic;

namespace Marketplace.Domain.Entities
{
    public class Faq : BaseEntity
    {
        public string title { get; set; }
        public string sub_title { get; set; }
        public IEnumerable<FaqQuestion> Question { get; set; }
    }
    public class FaqQuestion : BaseEntity
    {
        public string question { get; set; }
        public string ans { get; set; }
        public int faq_id { get; set; }
        public Faq Faq { get; set; }
    }
}