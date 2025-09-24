using ItemManagement.WebAPI.Models;

namespace ItemManagement.WebAPI.Services.Interfaces
{
    public interface ICategoriesApiService
    {
        Task<IEnumerable<CategoryApiModel>> GetCategoriesAsync();
        Task<CategoryApiModel> GetCategoryAsync(int id);
        Task<bool> UpdateCategoryAsync(CategoryApiModel Category);
        Task<bool> AddCategoryAsync(CategoryApiModel Category);
        Task<(bool Success, string? ErrorMessage)> DeleteCategoryAsync(CategoryApiModel categoryApiModel);
    }
}