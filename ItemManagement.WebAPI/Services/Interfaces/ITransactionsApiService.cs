using ItemManagement.WebAPI.Models;

namespace ItemManagement.WebAPI.Services.Interfaces
{
    public interface ITransactionsApiService
    {
        Task<IEnumerable<TransactionApiModel>> GetTransactionsAsync();
        Task<TransactionApiModel> GetTransactionAsync(int transactionId);
        Task<bool> RecordTransactionAsync(TransactionApiModel transaction);
        Task<bool> DeleteTransactionAsync(int transactionId);
        Task<bool> RecordTransactionAsync(SellOrderApiModel request);
    }
}
