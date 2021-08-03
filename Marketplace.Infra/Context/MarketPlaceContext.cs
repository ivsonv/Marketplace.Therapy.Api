using Marketplace.Domain.Entities;
using Marketplace.Infra.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Context
{
    public class MarketPlaceContext : DbContext
    {
        public MarketPlaceContext(DbContextOptions<MarketPlaceContext> options) : base(options) { }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddress { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderAddress> ProviderAddress { get; set; }
        public DbSet<ProviderBankAccount> ProviderBankAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>(new CategoryMap().Configure);
            modelBuilder.Entity<Customer>(new CustomerMap().Configure);
            modelBuilder.Entity<Language>(new LanguageMap().Configure);
            modelBuilder.Entity<Topic>(new TopicMap().Configure);
            modelBuilder.Entity<Appointment>(new AppointmentMap().Configure);
            modelBuilder.Entity<CustomerAddress>(new CustomerAddressMap().Configure);
            modelBuilder.Entity<CustomerAssessment>(new CustomerAssessmentMap().Configure);
            modelBuilder.Entity<Provider>(new ProviderMap().Configure);
            modelBuilder.Entity<ProviderAddress>(new ProviderAddressMap().Configure);
            modelBuilder.Entity<ProviderBankAccount>(new ProviderBankAccountMap().Configure);
            modelBuilder.Entity<ProviderCategories>(new ProviderCategoriesMap().Configure);
            modelBuilder.Entity<ProviderLanguages>(new ProviderLanguagesMap().Configure);
            modelBuilder.Entity<ProviderSchedule>(new ProviderScheduleMap().Configure);
            modelBuilder.Entity<ProviderSplitAccount>(new ProviderSplitAccountMap().Configure);
            modelBuilder.Entity<ProviderTopics>(new ProviderTopicsMap().Configure);
        }
    }
}
