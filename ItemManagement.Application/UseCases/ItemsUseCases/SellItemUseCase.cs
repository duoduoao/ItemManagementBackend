using AutoMapper;
using ItemManagement.Application.UseCaseInterfaces; 
using ItemManagement.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.Application.UseCases
{
    public class SellItemUseCase : ISellItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionsUseCases _transactionUseCase;

        public SellItemUseCase(IUnitOfWork unitOfWork, IMapper mapper, ITransactionsUseCases transactionUseCase)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _transactionUseCase = transactionUseCase;
        }

        public async Task ExecuteAsync(string cashierName, int itemId, int qtyToSell, CancellationToken cancellationToken = default)
        {
            var userId = cashierName;
            var item = await _unitOfWork.Item.GetItemByIdAsync(itemId, userId, cancellationToken);
            if (item == null) return;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                await _transactionUseCase.RecordTransactionAsync(cashierName, itemId, qtyToSell);

                item.Quantity -= qtyToSell;
                await _unitOfWork.Item.UpdateItemAsync(item, userId,cancellationToken);

                await _unitOfWork.SaveAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
