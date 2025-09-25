using FluentValidation;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Application.Contract;
using ItemManagement.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.Application.Common.Validation
{
    public class SellOrderDtoValidator : AbstractValidator<SellOrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;  // add user context service

        public SellOrderDtoValidator(IUnitOfWork unitOfWork, IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;

            RuleFor(x => x.CashierName)
                .NotEmpty().WithMessage("Cashier name is required.");

            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Valid ItemId is required.");

            RuleFor(x => x.SellQty)
                .GreaterThan(0).WithMessage("Sell quantity must be greater than zero.");

            RuleFor(x => x).CustomAsync(SellQtyNotExceedStock);
        }

        private async Task SellQtyNotExceedStock(SellOrderDto dto, ValidationContext<SellOrderDto> context, CancellationToken cancellationToken)
        {
            // get userId from injected user context service
            var userId = _userContext.UserId;

            var item = await _unitOfWork.Item.GetItemByIdAsync(dto.ItemId, userId, cancellationToken);
            if (item == null)
            {
                context.AddFailure("ItemId", "Item does not exist.");
                return;
            }
            if (item.Quantity < dto.SellQty)
            {
                context.AddFailure("SellQty", "Sell quantity cannot exceed available stock.");
            }
        }
    }
}
