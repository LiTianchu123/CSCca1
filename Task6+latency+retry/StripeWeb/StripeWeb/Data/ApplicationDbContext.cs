using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StripeWeb.Models;

namespace StripeWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<PaymentMethod>().ToTable("PaymentMethod");
            modelBuilder.Entity<Subscription>().ToTable("Subscription");
            modelBuilder.Entity<Transaction>().ToTable("Transaction");

        }
    }
}
