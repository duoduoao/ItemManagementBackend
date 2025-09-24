using AutoMapper;
using ItemManagement.Application.UserCases;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.Application.UseCasesInterfaces;
using ItemManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Application.UseCases
{
    public class SellItemUseCase : ISellItemUseCase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionsUseCases _transactionUseCase;  // Add this line

        public SellItemUseCase(IUnitOfWork unitOfWork, IMapper mapper, ITransactionsUseCases transactionUseCase)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
              _transactionUseCase = transactionUseCase;  // Initialize here

        }

        public async Task ExecuteAsync(string cashierName, int itemId, int qtyToSell)
        {
            var item = await _unitOfWork.Item.GetItemByIdAsync(itemId);
            if (item == null) return;

            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _transactionUseCase.RecordTransactionAsync(cashierName, itemId, qtyToSell);

                item.Quantity -= qtyToSell;
                _unitOfWork.Item.UpdateItem(item);

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }


        }
    }
}
