using System.Collections.Generic;

namespace Marketplace.Integrations.Payment.Nexxera.repository
{
    public class NXerrors
    {
        public List<string> errors { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}
