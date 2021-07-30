using System;

namespace Marketplace.Domain.Entities
{
    public class BaseEntity
    {
        public int id { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
