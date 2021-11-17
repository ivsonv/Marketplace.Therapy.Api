using System.Collections.Generic;

namespace Marketplace.Integrations.Payment.Nexxera.repository
{
    public class MerchantRq
    {
        public MerchantRq()
        {
            this.Contacts = new List<Contact>();
        }
        public List<Contact> Contacts { get; set; }
        public BankAccount BankAccount { get; set; }
        public Address Address { get; set; }
        public SandboxNotificationHooks SandboxNotificationHooks { get; set; }
        public ProductionNotificationHooks ProductionNotificationHooks { get; set; }
        public string CardRepositoryId { get; set; }
        public string SoftDescriptor { get; set; }
        public string CorporateName { get; set; }
        public string SocialNumberType { get; set; }
        public string SocialNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string emailAdminUser { get; set; }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneExtension { get; set; }
        public string Email { get; set; }
    }

    public class Holder
    {
        public string Name { get; set; }
        public string SocialNumber { get; set; }
    }

    public class BankAccount
    {
        public string BankId { get; set; }
        public string Branch { get; set; }
        public string BranchDigit { get; set; }
        public string Number { get; set; }
        public string Digit { get; set; }
        public string AccountType { get; set; }
        public Holder Holder { get; set; }
    }

    public class Address
    {
        public string Neighborhood { get; set; }
        public string Number { get; set; }
        public string AddressLine2 { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class SandboxNotificationHooks
    {
        public string OnCardTransactionChange { get; set; }
        public string OnBoletoTransactionChange { get; set; }
        public string OnRecurrenceChange { get; set; }
    }
    public class ProductionNotificationHooks
    {
        public string OnCardTransactionChange { get; set; }
        public string OnBoletoTransactionChange { get; set; }
        public string OnRecurrenceChange { get; set; }
    }


}
