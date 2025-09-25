using ItemManagement.Application.Contract;
using ItemManagement.Domain.Entities;
using ItemManagement.Domain.Repositories;
using ItemManagement.Infrastructure.Data;
using ItemManagement.Infrastructure.Repository;
using ItemManagement.Infrastructure.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ItemManagement.Infrastructure.Tests.Repository
{
    public class ItemRepositoryTests : IDisposable
    {
    private readonly ApplicationDbContext _context;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly ItemRepository _repository;

    public ItemRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB per test
            .Options;

        _context = new ApplicationDbContext(options);
        _cacheServiceMock = new Mock<ICacheService>();

        _repository = new ItemRepository(_context, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task AddItemAsync_AddsItem_And_RemovesCache()
    {
        var item = new Item { Name = "Test Item" };
        _cacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        await _repository.AddItemAsync(item, "user1");

        var added = await _context.Items.FirstOrDefaultAsync(i => i.Name == "Test Item");
        Assert.NotNull(added);
        _cacheServiceMock.Verify(c => c.RemoveAsync(It.Is<string>(k => k.Contains("items_all_user1"))), Times.Once);
    }

    [Fact]
    public async Task DeleteItemAsync_RemovesItem_And_ClearsCache()
    {
        var item = new Item { Name = "Delete Item" };
        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        _cacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        bool deleted = await _repository.DeleteItemAsync(item.ItemId, "user1");

        Assert.True(deleted);
        var exists = await _context.Items.FindAsync(item.ItemId);
        Assert.Null(exists);
        _cacheServiceMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Exactly(2)); // item and all cache keys
    }

    [Fact]
    public async Task GetItemByIdAsync_ReturnsCachedItem_IfExists()
    {
        var cachedItem = new Item { ItemId = 1, Name = "Cached Item" };
        _cacheServiceMock.Setup(c => c.GetAsync<Item>(It.IsAny<string>()))
            .ReturnsAsync(cachedItem);

        var result = await _repository.GetItemByIdAsync(1, "user1");

        Assert.Equal(cachedItem.Name, result.Name);
        _cacheServiceMock.Verify(c => c.GetAsync<Item>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetItemsAsync_ReturnsCachedItemList_IfExists()
    {
        var cachedItems = new List<Item> { new Item { Name = "Cached1" }, new Item { Name = "Cached2" } };
        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<Item>>(It.IsAny<string>()))
            .ReturnsAsync(cachedItems);

        var result = await _repository.GetItemsAsync("user1");

        Assert.Equal(2, ((List<Item>)result).Count);
        _cacheServiceMock.Verify(c => c.GetAsync<IEnumerable<Item>>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetItemsByCategoryId_ReturnsFilteredIQueryable()
    {
        _context.Items.AddRange(
            new Item { CategoryId = 1, Name = "Item1" },
            new Item { CategoryId = 2, Name = "Item2" });
        await _context.SaveChangesAsync();

        var query = _repository.GetItemsByCategoryId(1);
        var list = await query.ToListAsync();

        Assert.Single(list);
        Assert.Equal("Item1", list[0].Name);
    }

    [Fact]
    public async Task UpdateItemAsync_UpdatesProperties_And_UpdatesCache()
    {
        var item = new Item { ItemId = 1, Name = "Old", CategoryId = 1, Price = 10, Quantity = 5 };
        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        var updatedItem = new Item { ItemId = 1, Name = "New", CategoryId = 2, Price = 20, Quantity = 10 };
        _cacheServiceMock.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<Item>(), It.IsAny<TimeSpan>())).Returns(Task.CompletedTask);
        _cacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        await _repository.UpdateItemAsync(updatedItem, "user1");

        var dbItem = await _context.Items.FindAsync(1);
        Assert.Equal("New", dbItem.Name);
        Assert.Equal(2, dbItem.CategoryId);
        Assert.Equal(20, dbItem.Price);
        Assert.Equal(10, dbItem.Quantity);

        _cacheServiceMock.Verify(c => c.SetAsync(It.Is<string>(k => k.Contains("item_user1_1")), dbItem, It.IsAny<TimeSpan>()), Times.Once);
        _cacheServiceMock.Verify(c => c.RemoveAsync(It.Is<string>(k => k.Contains("items_all_user1"))), Times.Once);
    }


    public void Dispose()
    {
        _context.Dispose();
    }
}
}