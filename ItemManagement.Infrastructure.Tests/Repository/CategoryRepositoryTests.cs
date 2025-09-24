using ItemManagement.Domain.Entities;
using ItemManagement.Infrastructure.Data;
using ItemManagement.Infrastructure.Repository;
using ItemManagement.Infrastructure.Tests.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ItemManagement.Infrastructure.Tests.Repository
{
    public class CategoryRepositoryTests
    {
        private ApplicationDbContextTests GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContextTests>()
                .UseInMemoryDatabase(databaseName: "TestDb_CategoryRepo_" + Guid.NewGuid())
                .Options;

            var context = new ApplicationDbContextTests(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task AddCategory_ShouldAddCategory()
        {
            await using var context = GetInMemoryDbContext();
            var repo = new CategoryRepository(context);

            var category = new Category
            {
                Name = "New Category",
                Description = "Test description"
            };

            repo.AddCategory(category);
            await context.SaveChangesAsync();

            var added = await context.Categories.FirstOrDefaultAsync(c => c.Name == "New Category");
            Assert.NotNull(added);
            Assert.Equal("New Category", added.Name);
        }

        [Fact]
        public async Task DeleteCategory_ShouldDeleteCategory()
        {
            await using var context = GetInMemoryDbContext();
            var repo = new CategoryRepository(context);

            var category = new Category { Name = "ToDelete", Description = "Delete me" };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var deleted = repo.DeleteCategory(category.CategoryId);
            await context.SaveChangesAsync();

            Assert.True(deleted);
            var check = await context.Categories.FindAsync(category.CategoryId);
            Assert.Null(check);
        }

        [Fact]
        public async Task UpdateCategory_ShouldPersistChanges()
        {
            await using var context = GetInMemoryDbContext();
            var repo = new CategoryRepository(context);

            var category = new Category { Name = "OldName", Description = "Old Desc" };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            category.Name = "UpdatedName";
            category.Description = "Updated Desc";

            repo.UpdateCategory(category);
            await context.SaveChangesAsync();

            var updated = await context.Categories.FindAsync(category.CategoryId);
            Assert.NotNull(updated);
            Assert.Equal("UpdatedName", updated.Name);
            Assert.Equal("Updated Desc", updated.Description);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnAll()
        {
            await using var context = GetInMemoryDbContext();
            var repo = new CategoryRepository(context);

            await context.Categories.AddRangeAsync(
                new Category { Name = "Cat1", Description = "Desc1" },
                new Category { Name = "Cat2", Description = "Desc2" }
            );
            await context.SaveChangesAsync();

            var categories = await repo.GetCategoriesAsync();

            Assert.Equal(2, categories.Count());
            Assert.Contains(categories, c => c.Name == "Cat1");
            Assert.Contains(categories, c => c.Name == "Cat2");
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnCorrectCategory()
        {
            await using var context = GetInMemoryDbContext();
            var repo = new CategoryRepository(context);

            var category = new Category { Name = "FindMe", Description = "Find me description" };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var found = await repo.GetCategoryByIdAsync(category.CategoryId);

            Assert.NotNull(found);
            Assert.Equal("FindMe", found.Name);
        }
    }
}
