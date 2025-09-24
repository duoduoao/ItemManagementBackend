using FluentValidation;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Application.Common.Validation
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            // Other property rules
            RuleFor(x => x.Name).NotEmpty();

            // Custom async rule for checking linked items before delete
            RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
             !await _unitOfWork.Item.GetItemsByCategoryId(dto.CategoryId).AnyAsync(cancellation))
             .When(x => x.IsBeingDeleted)
             .WithMessage("Category cannot be deleted if items are linked to it.");
        }
    }


}
