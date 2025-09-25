using ItemManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.Domain.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        Task AddItemAsync(Item item, string userId, CancellationToken cancellationToken = default);
        Task<bool> DeleteItemAsync(int itemId, string userId, CancellationToken cancellationToken = default);
        Task<Item> GetItemByIdAsync(int itemId, string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Item>> GetItemsAsync(string userId, CancellationToken cancellationToken = default);
        IQueryable<Item> GetItemsByCategoryId(int categoryId);
        Task UpdateItemAsync(Item item, string userId, CancellationToken cancellationToken = default);
    }
}
