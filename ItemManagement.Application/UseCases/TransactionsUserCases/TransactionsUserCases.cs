using AutoMapper;
using ItemManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.Application.UseCasesInterfaces;
using ItemManagement.Application.Common.DTO;

namespace ItemManagement.Application.UserCases
{
    public class TransactionsUseCases : ITransactionsUseCases
    //   :
    //IGetTodayTransactionsUseCase,
    //IRecordTransactionUseCase,
    //IGetTransactionsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemsUseCases _getItemByIdUseCase;
        private readonly IMapper _mapper;

        public TransactionsUseCases(
            IUnitOfWork unitOfWork,
            IItemsUseCases getItemByIdUseCase,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _getItemByIdUseCase = getItemByIdUseCase;
            _mapper = mapper;
        }

        public IEnumerable<TransactionDto> GetTodayTransactions(string cashierName)
        {
            var transactions = _unitOfWork.Transaction.GetByDay(cashierName, DateTime.Now);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task RecordTransactionAsync(string cashierName, int itemId, int qty)
        {
            var item =  await _getItemByIdUseCase.GetItemByIdUseCase(itemId);
            _unitOfWork.Transaction.Save(cashierName, itemId, item.Name, item.Price, item.Quantity, qty);
            await _unitOfWork.SaveAsync();  // Persist changes through UnitOfWork
        }

        public IEnumerable<TransactionDto> GetTransactions(string cashierName, DateTime startDate, DateTime endDate)
        {
            var transactions = _unitOfWork.Transaction.Search(cashierName, startDate, endDate);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public IEnumerable<TransactionDto> GetAllTransactions()
        {
            var transactions = _unitOfWork.Transaction.GetAll(); // Add GetAll method if not present
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }
    }

}
