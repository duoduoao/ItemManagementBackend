using ItemManagement.Domain;
using ItemManagement.Infrastructure.Data;
using ItemManagement.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;
namespace ItemManagement.Infrastructure.Tests.Data
{
    public class ApplicationDbContextTests : ApplicationDbContext
    {
        public ApplicationDbContextTests(DbContextOptions options) : base(options)
        {
        }

        // Override to skip seed data in tests
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply only configurations without seeding
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            // Notice: NO seeding calls like HasData here
        }
    }
    }