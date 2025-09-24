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
    public class ItemRepositoryTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContextTests>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            var context = new ApplicationDbContextTests(options);
            // Clean database before each test
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GivenNewItem_WhenAddItem_ThenItemIsAdded()
        {
            await using var context = GetInMemoryDbContext();
            var repo = new ItemRepository(context);

            var item = new Item
            {
                CategoryId = 1,
                Name = "Test Item",
                Quantity = 10,
                Price = 19.99
            };

            repo.AddItem(item);
            await context.SaveChangesAsync();

            var inserted = await context.Items.FirstOrDefaultAsync(i => i.Name == "Test Item");
            Assert.NotNull(inserted);
            Assert.Equal("Test Item", inserted.Name);
        }

        [Fact]
        public async Task GivenExistingItem_WhenDeleteItem_ThenItemIsDeleted()
        {
            await using var context = GetInMemoryDbContext();
            var repo = new ItemRepository(context);

            var item = new Item { CategoryId = 1, Name = "ToDelete", Quantity = 5, Price = 10 };
            await context.Items.AddAsync(item);
            await context.SaveChangesAsync();

            var result = repo.DeleteItem(item.ItemId);
            await context.SaveChangesAsync();

            Assert.True(result);
            var deleted = await context.Items.FindAsync(item.ItemId);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task GivenValidItemId_WhenGetItemByIdAsync_ThenReturnsItem()
        {
            await using var context = GetInMemoryDbContext();
            var repo = new ItemRepository(context);

            var item = new Item { CategoryId = 2, Name = "GetById", Quantity = 7, Price = 5 };
            await context.Items.AddAsync(item);
            await context.SaveChangesAsync();

            var result = await repo.GetItemByIdAsync(item.ItemId);

            Assert.NotNull(result);
            Assert.Equal("GetById", result.Name);
        }

        [Fact]
        public async Task GivenCategoryId_WhenGetItemsByCategoryId_ThenReturnMatchingItems()
        {
            await using var context = GetInMemoryDbContext();
            var repo = new ItemRepository(context);

            await context.Items.AddRangeAsync(
                new Item { CategoryId = 3, Name = "Item1", Quantity = 1, Price = 2 },
                new Item { CategoryId = 3, Name = "Item2", Quantity = 1, Price = 3 },
                new Item { CategoryId = 1, Name = "Item3", Quantity = 1, Price = 4 }
            );
            await context.SaveChangesAsync();

            var results = repo.GetItemsByCategoryId(3).ToList();

            Assert.Equal(2, results.Count);
            Assert.All(results, i => Assert.Equal(3, i.CategoryId));
        }

        [Fact]
        public async Task GivenUpdatedItem_WhenUpdateItem_ThenPersistsChanges()
        {
            await using var context = GetInMemoryDbContext();
            var repo = new ItemRepository(context);

            var item = new Item { CategoryId = 2, Name = "BeforeUpdate", Quantity = 2, Price = 20 };
            await context.Items.AddAsync(item);
            await context.SaveChangesAsync();

            var updatedItem = new Item
            {
                ItemId = item.ItemId,
                CategoryId = 2,
                Name = "AfterUpdate",
                Quantity = 5,
                Price = 25
            };

            repo.UpdateItem(updatedItem);
            await context.SaveChangesAsync();

            var result = await context.Items.FindAsync(item.ItemId);

            Assert.NotNull(result);
            Assert.Equal("AfterUpdate", result!.Name);
            Assert.Equal(5, result.Quantity);
            Assert.Equal(25, result.Price);
        }

        [Fact]
        public async Task GivenItems_WhenGetItems_ThenReturnAll()
        {
            await using var context = GetInMemoryDbContext();

            await context.Items.AddRangeAsync(
                new Item { Name = "Item1", Price = 10.0, Quantity = 5, CategoryId = 1 },
                new Item { Name = "Item2", Price = 15.0, Quantity = 3, CategoryId = 1 }
            );
            await context.SaveChangesAsync();

            var repo = new ItemRepository(context);
            var items = await repo.GetItemsAsync();

            Assert.Equal(2, items.Count());
        }
    }
}
