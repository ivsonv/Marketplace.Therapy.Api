namespace Marketplace.Domain.Helpers
{
    public static class Enumerados
    {
        public enum UserRule
        {
            admin = 0,
            coordenador = 2,
            employee = 3,
            customer = 4
        }

        public enum ProviderStatus
        {
            pending = 0,
            approved = 2,
            blocked = 3,
            disabled = 4
        }

        public enum CompanyFilialStatus
        {
            pending = 0,
            approved = 2,
            blocked = 3
        }

        public enum LocalityProvider
        {
            correios = 0,
            kinghost = 1
        }

        public enum EmailType
        {
            welcome,
            recoverpassword
        }

        public enum UploadFileType
        {
            image = 0,
            others = 1
        }

        public enum PaymentMethod
        {
            pix = 0,
            boleto = 1,
            creditcard = 2,
            debitcard = 3,
            transfer = 4
        }

        public enum CardBrand
        {
            mastercard = 0,
            vida = 1,
            dinners = 2,
            american_express = 3,
            hiper = 4,
            hipercard = 5,
            Elo = 6
        }
    }
}