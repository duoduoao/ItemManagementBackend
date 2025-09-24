using ItemManagement.Domain.Entities;
using ItemManagement.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;


namespace ItemManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
       
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());

            // seeding some data
            modelBuilder.Entity<Category>().HasData(
                    new Category { CategoryId = 1, Name = "Book", Description = "Book" },
                    new Category { CategoryId = 2, Name = "Toy", Description = "Toy" },
                    new Category { CategoryId = 3, Name = "Cloth", Description = "Cloth" }
                );

            modelBuilder.Entity<Item>().HasData(
                    new Item { ItemId = 1, CategoryId = 1, Name = "Dog Man", Quantity = 1, Price = 12.99 },
                    new Item { ItemId = 2, CategoryId = 2, Name = "Car", Quantity = 2, Price = 5.00 },
                    new Item { ItemId = 3, CategoryId = 3, Name = "Uniform", Quantity = 3, Price = 35.50 },
                    new Item { ItemId = 4, CategoryId = 1, Name = "Bad Guy", Quantity = 1, Price = 11.50 }
                );
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
