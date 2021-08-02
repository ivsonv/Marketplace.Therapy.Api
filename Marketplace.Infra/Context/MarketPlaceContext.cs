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
            modelBuilder.Entity<CustomerAddress>(new CustomerAddressMap().Configure);
            modelBuilder.Entity<Provider>(new ProviderMap().Configure);
            modelBuilder.Entity<ProviderAddress>(new ProviderAddressMap().Configure);
            modelBuilder.Entity<ProviderBankAccount>(new ProviderBankAccountMap().Configure);
        }
    }
}
