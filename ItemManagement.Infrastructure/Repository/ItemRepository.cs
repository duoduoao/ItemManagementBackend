using ItemManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ItemManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ItemManagement.Domain.Repositories;
using ItemManagement.Application.Contract;

namespace ItemManagement.Infrastructure.Repository
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ICacheService _cacheService;

        public ItemRepository(ApplicationDbContext db, ICacheService cacheService) : base(db)
        {
            _db = db;
            _cacheService = cacheService;
        }

        private string GetCacheKey(int itemId, string userId) => $"item_{userId}_{itemId}";
        private string GetCacheKeyAll(string userId) => $"items_all_{userId}";

        public async Task AddItemAsync(Item item, string userId, CancellationToken cancellationToken = default)
        {
            await _db.Items.AddAsync(item, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            await _cacheService.RemoveAsync(GetCacheKeyAll(userId));
        }

        public async Task<bool> DeleteItemAsync(int itemId, string userId, CancellationToken cancellationToken = default)
        {
            var item = await _db.Items.FindAsync(new object[] { itemId }, cancellationToken);
            if (item == null) return false;

            _db.Items.Remove(item);
            await _db.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync(GetCacheKey(itemId, userId));
            await _cacheService.RemoveAsync(GetCacheKeyAll(userId));

            return true;
        }

        public async Task<Item> GetItemByIdAsync(int itemId, string userId, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(itemId, userId);
            var cachedItem = await _cacheService.GetAsync<Item>(cacheKey);
            if (cachedItem != null) return cachedItem;

            var item = await _db.Items.FindAsync(new object[] { itemId }, cancellationToken);
            if (item != null)
                await _cacheService.SetAsync(cacheKey, item, TimeSpan.FromMinutes(10));

            return item;
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKeyAll(userId);
            var cachedItems = await _cacheService.GetAsync<IEnumerable<Item>>(cacheKey);
            if (cachedItems != null) return cachedItems;

            var items = await _db.Items.ToListAsync(cancellationToken);
            await _cacheService.SetAsync(cacheKey, items, TimeSpan.FromMinutes(10));
            return items;
        }

        public IQueryable<Item> GetItemsByCategoryId(int categoryId)
        {
            return _db.Items.Where(x => x.CategoryId == categoryId);
        }

        public async Task UpdateItemAsync(Item item, string userId, CancellationToken cancellationToken = default)
        {
            var dbItem = await _db.Items.FindAsync(new object[] { item.ItemId }, cancellationToken);
            if (dbItem == null) return;

            dbItem.CategoryId = item.CategoryId;
            dbItem.Name = item.Name;
            dbItem.Price = item.Price;
            dbItem.Quantity = item.Quantity;

            await _db.SaveChangesAsync(cancellationToken);

            await _cacheService.SetAsync(GetCacheKey(item.ItemId, userId), dbItem, TimeSpan.FromMinutes(10));
            await _cacheService.RemoveAsync(GetCacheKeyAll(userId));
        }
    }
}
