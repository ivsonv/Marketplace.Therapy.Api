namespace Marketplace.Integrations.Payment.Nexxera.repository
{
    public class MerchantRs : NXerrors
    {
        public Sandbox sandbox { get; set; }
        public Production production { get; set; }
    }

    public class Sandbox
    {
        public string accessKey { get; set; }
        public string apiSecretKey { get; set; }
    }

    public class Production
    {
        public string accessKey { get; set; }
        public string apiSecretKey { get; set; }
    }
}
