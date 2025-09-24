using ItemManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ItemManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemManagement.Domain.Repositories;

namespace ItemManagement.Infrastructure.Repository
{
    public class ItemRepository :Repository<Item>, IItemRepository
    {
        private readonly ApplicationDbContext db;

        public ItemRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public void AddItem(Item Item)
        {
            db.Items.Add(Item);
           // db.SaveChanges();
        }

        public bool DeleteItem(int ItemId)
        {
         
            try
            {
                var Item = db.Items.Find(ItemId);
                //   if (category == null) return;
                if (Item != null)
                {
                    db.Items.Remove(Item);
          //          db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            } 
        }

        public async Task<Item> GetItemByIdAsync(int ItemId)
        {
            return await db.Items.FindAsync(ItemId);
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await db.Items.ToListAsync();
        }

        public   IQueryable<Item> GetItemsByCategoryId(int categoryId)
        {
            return db.Items
                .Where(x => x.CategoryId == categoryId);
        }

        public void UpdateItem(Item Item)
        {
            var prod = db.Items.Find(Item.ItemId);
            prod.CategoryId = Item.CategoryId;
            prod.Name = Item.Name;
            prod.Price = Item.Price;
            prod.Quantity = Item.Quantity;

           // db.SaveChanges();
        }
    }
}
