using System;
using System.Collections.Generic;

namespace Marketplace.Integrations.Payment.Nexxera.repository
{
    public class CancelRq
    {
        public string paymentToken { get; set; }
        public string amount { get; set; }
    }
}