﻿namespace Marketplace.Domain.Helpers
{
    public static class Enumerados
    {
        public enum AccountBankType
        {
            contaCorrente = 0,
            contaPoupanca = 1,
            contaPrePaga = 2
        }
        public enum PaymentStatus
        {
            pending = 0,
            confirmed = 1,
            canceled = 2,
            notAuthorized = 3
        }

        public enum AppointmentStatus
        {
            pending = 0,
            confirmed = 1,
            canceled = 2,
            notAuthorized = 3
        }

        public enum AppointmentOrigin
        {
            offline = 0,
            ecommerce = 1,
            mobile = 2
        }

        public enum BannerType
        {
            carrossel = 0,
            assessment = 1
        }

        public enum AppointmentType
        {
            online_session = 0,
            personal_session = 1,
            //private_session = 2
        }

        public enum ProviderStatus
        {
            pending = 0,
            approved = 1,
            blocked = 2,
            others = 3
        }

        public enum ProviderGender
        {
            men = 0,
            women = 1,
            notbinary = 2
        }

        public enum PaymentProvider
        {
            nexxera = 0
        }

        public enum LocalityProvider
        {
            correios = 0,
            kinghost = 1
        }

        public enum EmailType
        {
            welcome,
            recoverpassword,
            appointment,
            nexxera,
            defaultt
        }

        public enum UploadFileType
        {
            image = 0,
            others = 1
        }

        public enum PaymentMethod
        {
            creditcard = 0,
            boleto = 1,
            pix = 2,
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