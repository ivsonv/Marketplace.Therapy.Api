using Marketplace.Domain.Entities;
using Marketplace.Infra.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Context
{
    public class MarketPlaceContext : DbContext
    {
        public MarketPlaceContext(DbContextOptions<MarketPlaceContext> options) : base(options)
        {
            base.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddress { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderAddress> ProviderAddress { get; set; }
        public DbSet<ProviderBankAccount> ProviderBankAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GroupPermission>(new GroupPermissionMap().Configure);
            modelBuilder.Entity<GroupPermissionAttached>(new GroupPermissionAttachedMap().Configure);
            modelBuilder.Entity<User>(new UserMap().Configure);
            modelBuilder.Entity<UserGroupPermission>(new UserGroupPermissionMap().Configure);
            modelBuilder.Entity<Bank>(new BankMap().Configure);

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
            modelBuilder.Entity<ProviderReceipt>(new ProviderReceiptMap().Configure);
            modelBuilder.Entity<AppointmentLog>(new AppointmentLogMap().Configure);
        }
    }
}
