using ItemManagement.Domain.Entities;
using ItemManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ItemManagement.Infrastructure.Repository
{
    public class CategoryRepository :Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public void AddCategory(Category category)
        {
            db.Categories.Add(category);
       //     db.SaveChanges();
        }

        public bool DeleteCategory(int categoryId)
        {
            try
            {
                var category = db.Categories.Find(categoryId);
                //   if (category == null) return;
                if (category != null)
                {
                    db.Categories.Remove(category);
          //          db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await db.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await db.Categories.FindAsync(categoryId);
        }

        public void UpdateCategory(Category category)
        {
            var cat = db.Categories.Find(category.CategoryId);
            cat.Name = category.Name;
            cat.Description = category.Description;
     //       db.SaveChanges();
        }
    }
}
