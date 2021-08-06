using System.Collections.Generic;

namespace Marketplace.Domain.Models.dto.auth
{
    public class AuthDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public IEnumerable<string> roles { get; set; }
        public IEnumerable<int> permissions { get; set; }
    }
}
