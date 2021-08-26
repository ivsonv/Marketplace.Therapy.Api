using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.auth
{
    public class AuthData
    {
        public string fullName { get; set; }
        public IEnumerable<string> roles { get; set; }
        public int id { get; set; }
        public string avatar { get; set; }
    }
}
