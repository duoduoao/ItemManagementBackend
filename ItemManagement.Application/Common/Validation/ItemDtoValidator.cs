using FluentValidation;
using ItemManagement.Application.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Application.Common.Validation
{
    public class ItemDtoValidator : AbstractValidator<ItemDto>
    {
        public ItemDtoValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}
