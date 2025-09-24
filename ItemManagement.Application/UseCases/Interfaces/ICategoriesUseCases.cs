using ItemManagement.Application.Common;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Domain.Entities;
using System.Collections.Generic;

using System.Threading.Tasks;
namespace ItemManagement.Application.UseCaseInterfaces
{
    public interface ICategoriesUseCases
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesUseCase();
        Task<CategoryDto> GetCategoryByIdUseCase(int id);
        Task AddCategoryUseCase(CategoryDto category);
        Task<bool> EditCategoryUseCase(CategoryDto category);
        Task<DeleteCategoryResult> DeleteCategoryUseCase(CategoryDto category);

    }
}