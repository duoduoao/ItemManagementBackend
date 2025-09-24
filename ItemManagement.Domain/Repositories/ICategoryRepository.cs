using ItemManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
         void AddCategory(Category category);
        void UpdateCategory(Category category);
        Task<Category> GetCategoryByIdAsync(int categoryId);
        bool DeleteCategory(int categoryId);
    }
}
