using ItemManagement.Application.Common.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

 
    using ItemManagement.Application.Common.DTO;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    namespace ItemManagement.Application.UseCaseInterfaces
    {
        public interface ITransactionsUseCases
        {
            IEnumerable<TransactionDto> GetTodayTransactions(string cashierName);

            Task RecordTransactionAsync(string cashierName, int itemId, int qty, CancellationToken cancellationToken = default);

            IEnumerable<TransactionDto> GetTransactions(string cashierName, DateTime startDate, DateTime endDate);

            IEnumerable<TransactionDto> GetAllTransactions();
        }
    }

 