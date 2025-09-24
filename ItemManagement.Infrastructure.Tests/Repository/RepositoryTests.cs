using ItemManagement.Domain.Entities;
using ItemManagement.Infrastructure.Data;
using ItemManagement.Infrastructure.Repository;
using ItemManagement.Infrastructure.Tests.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace ItemManagement.Infrastructure.Tests.Repository
{
    public class RepositoryTests
    {
        private ApplicationDbContextTests GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContextTests>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;
            var context = new ApplicationDbContextTests(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public void Add_ShouldAddEntity()
        {
            using var context = GetInMemoryDbContext();
            var repo = new Repository<Category>(context);
            var category = new Category { Name = "Test Category", Description = "Desc" };
            repo.Add(category);
            context.SaveChanges();

            var inserted = context.Categories.Find(category.CategoryId);
            Assert.NotNull(inserted);
            Assert.Equal("Test Category", inserted.Name);
        }

        [Fact]
        public void Any_ShouldReturnTrueIfExists()
        {
            using var context = GetInMemoryDbContext();
            var repo = new Repository<Category>(context);
            context.Categories.Add(new Category { Name = "Check", Description = "Desc" });
            context.SaveChanges();

            var result = repo.Any(c => c.Name == "Check");
            Assert.True(result);
        }

        [Fact]
        public void Get_ShouldReturnEntityByFilter()
        {
            using var context = GetInMemoryDbContext();
            var repo = new Repository<Category>(context);
            context.Categories.Add(new Category { Name = "FindMe", Description = "Desc" });
            context.SaveChanges();

            var result = repo.Get(c => c.Name == "FindMe");
            Assert.NotNull(result);
            Assert.Equal("FindMe", result.Name);
        }

        [Fact]
        public void GetAll_ShouldReturnFilteredEntities()
        {
            using var context = GetInMemoryDbContext();
            var repo = new Repository<Category>(context);
            context.Categories.AddRange(
                new Category { Name = "Include", Description = "Desc" },
                new Category { Name = "Exclude", Description = "Desc" });
            context.SaveChanges();

            var results = repo.GetAll(c => c.Name == "Include");
            Assert.Single(results);
            Assert.Equal("Include", results.First().Name);
        }

        [Fact]
        public void Remove_ShouldDeleteEntity()
        {
            using var context = GetInMemoryDbContext();
            var repo = new Repository<Category>(context);
            var category = new Category { Name = "ToDelete", Description = "Desc" };
            context.Categories.Add(category);
            context.SaveChanges();

            repo.Remove(category);
            context.SaveChanges();

            var deleted = context.Categories.Find(category.CategoryId);
            Assert.Null(deleted);
        }
    }
}
