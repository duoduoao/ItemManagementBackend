using AutoMapper;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Application.UseCaseInterfaces; 
using ItemManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.Application.UseCases
{
    public class TransactionsUseCases : ITransactionsUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemsUseCases _itemsUseCases;
        private readonly IMapper _mapper;

        public TransactionsUseCases(
            IUnitOfWork unitOfWork,
            IItemsUseCases itemsUseCases,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _itemsUseCases = itemsUseCases;
            _mapper = mapper;
        }

        public IEnumerable<TransactionDto> GetTodayTransactions(string cashierName)
        {
            var transactions = _unitOfWork.Transaction.GetByDay(cashierName, DateTime.Now);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task RecordTransactionAsync(string cashierName, int itemId, int qty, CancellationToken cancellationToken = default)
        {
            var userId = cashierName; // Or obtain userId from context if different from cashierName

            var item = await _itemsUseCases.GetItemByIdUseCase(itemId, userId, cancellationToken);
            if (item == null)
            {
                throw new ArgumentException($"Item with id {itemId} not found.");
            }

            // Save the transaction record - assuming Save sync method is fine here
            _unitOfWork.Transaction.Save(cashierName, itemId, item.Name, item.Price, item.Quantity, qty);

            // Save changes asynchronously with cancellation token
            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public IEnumerable<TransactionDto> GetTransactions(string cashierName, DateTime startDate, DateTime endDate)
        {
            var transactions = _unitOfWork.Transaction.Search(cashierName, startDate, endDate);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public IEnumerable<TransactionDto> GetAllTransactions()
        {
            var transactions = _unitOfWork.Transaction.GetAll();
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }
    }
}
