using ItemManagement.Application.Common.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ItemManagement.Application.UseCasesInterfaces
{
    public interface ITransactionsUseCases
    {
        // Get all transactions for today, filtered by cashierName
        IEnumerable<TransactionDto> GetTodayTransactions(string cashierName);

        // Record a new transaction
        Task RecordTransactionAsync(string cashierName, int itemId, int qty);

        // Get transactions filtered by cashierName and date range
        IEnumerable<TransactionDto> GetTransactions(string cashierName, DateTime startDate, DateTime endDate);

        IEnumerable<TransactionDto> GetAllTransactions();

    }

}