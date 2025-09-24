using AutoMapper;
using FluentValidation;
using ItemManagement.Application.Common;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.Domain.Entities;
using ItemManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

namespace ItemManagement.Application.UseCases
{
    public class CategoriesUseCases : ICategoriesUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryDto> _categoryDtoValidator;

 
        public CategoriesUseCases(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CategoryDto> categoryDtoValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _categoryDtoValidator = categoryDtoValidator;
        }

        public async Task AddCategoryUseCase(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.Category.AddCategory(category);
            await _unitOfWork.SaveAsync();
        }

        //public async Task <DeleteCategoryResult> DeleteCategoryUseCase(int categoryId)
        //{ 
        //    var hasLinkedItems = await _unitOfWork.Item.GetItemsByCategoryId(categoryId).AnyAsync();
        //    if (hasLinkedItems)
        //        return DeleteCategoryResult.HasLinkedItems;

        //    var deleted = _unitOfWork.Category.DeleteCategory(categoryId);
        //    await _unitOfWork.SaveAsync();
        //    return deleted ? DeleteCategoryResult.Success : DeleteCategoryResult.NotFound;
        //}
        public async Task<DeleteCategoryResult> DeleteCategoryUseCase(CategoryDto categoryDto)
        {
            categoryDto.IsBeingDeleted = true;

            // Perform FluentValidation check including linked items check
            var validationResult = await _categoryDtoValidator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                // Usually map validation errors to enum or throw custom exception
                return DeleteCategoryResult.HasLinkedItems;
            }

            // Proceed with delete logic
            bool deleted = _unitOfWork.Category.DeleteCategory(categoryDto.CategoryId);
            if (!deleted)
                return DeleteCategoryResult.NotFound;

            await _unitOfWork.SaveAsync();

            return DeleteCategoryResult.Success;
        }

        public async Task< IEnumerable<CategoryDto>> GetCategoriesUseCase()
        {
            var categories = await _unitOfWork.Category.GetCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdUseCase(int categoryId)
        {
            var category = await _unitOfWork.Category.GetCategoryByIdAsync(categoryId);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task <bool> EditCategoryUseCase(CategoryDto categoryDto)
        {
            var categoryD = _unitOfWork.Category.GetCategoryByIdAsync(categoryDto.CategoryId);
            if (categoryD == null)
            {
                return false;  // signal "item not found"
            }
            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.Category.UpdateCategory(category);
            await _unitOfWork.SaveAsync();
            return true;
        }
      
       
           
    }
}
