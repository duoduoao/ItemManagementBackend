using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Application.Common.Validation
{
    using FluentValidation;
    using ItemManagement.Application.Common.DTO;

    public class TransactionDtoValidator : AbstractValidator<TransactionDto>
    {
        public TransactionDtoValidator()
        {
            RuleFor(x => x.TransactionId)
                .GreaterThan(0).WithMessage("TransactionId must be greater than 0.");

            RuleFor(x => x.TimeStamp)
                .NotEmpty().WithMessage("Timestamp is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Timestamp cannot be in the future.");

            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("ItemId must be greater than 0.");

            RuleFor(x => x.ItemName)
                .NotEmpty().WithMessage("Item name is required.")
                .MaximumLength(200).WithMessage("Item name cannot exceed 200 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.BeforeQty)
                .GreaterThanOrEqualTo(0).WithMessage("BeforeQty cannot be negative.");

            RuleFor(x => x.SoldQty)
                .GreaterThan(0).WithMessage("SoldQty must be greater than 0.");

            RuleFor(x => x.CashierName)
                .NotEmpty().WithMessage("Cashier name is required.")
                .MaximumLength(100).WithMessage("Cashier name cannot exceed 100 characters.");
        }
    }

}
