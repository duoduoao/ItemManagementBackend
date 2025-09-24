using ItemManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Domain.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetItemsAsync();
        void AddItem(Item Item);
        void UpdateItem(Item Item);
        Task<Item> GetItemByIdAsync(int ItemId);
      bool   DeleteItem(int ItemId);
   
        IQueryable<Item> GetItemsByCategoryId(int categoryId);
    }
}
