using System.Collections.Generic;

namespace Marketplace.Domain.Models.Response.auth
{
    public class AuthData
    {
        public string fullName { get; set; }
        public List<string> rules { get; set; }
        public int id { get; set; }
    }
}
