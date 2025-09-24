using FluentValidation;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.Application.Common.Validation
{ 
  public class SellOrderDtoValidator : AbstractValidator<SellOrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SellOrderDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.CashierName)
                .NotEmpty().WithMessage("Cashier name is required.");

            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Valid ItemId is required.");

            RuleFor(x => x.SellQty)
                .GreaterThan(0).WithMessage("Sell quantity must be greater than zero.")
                .MustAsync(SellQtyNotExceedStock)
                .WithMessage("Sell quantity cannot exceed available stock.");
        }

        private async Task<bool> SellQtyNotExceedStock(SellOrderDto dto, int sellQty, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.Item.GetItemByIdAsync(dto.ItemId);
            if (item == null) return false; // or throw if item must exist

            return item.Quantity >= sellQty;
        }
    }
}
