using ItemManagement.WebAPI.Models;
using ItemManagement.WebAPI.Services.Interfaces;

namespace ItemManagement.WebAPI.Services.Inplement
{
    public class TransactionsApiService : ITransactionsApiService
    {
        private readonly HttpClient _httpClient;

        public TransactionsApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> RecordTransactionAsync(SellOrderApiModel request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/transactionsapi/record", request);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
        public async Task<IEnumerable<TransactionApiModel>> GetTransactionsAsync()
        {
            var response = await _httpClient.GetAsync("api/transactionsapi");
            response.EnsureSuccessStatusCode();
            var transactions = await response.Content.ReadFromJsonAsync<IEnumerable<TransactionApiModel>>();

            if (transactions == null)
            {
                throw new InvalidOperationException("Failed to retrieve transactions. The response content is null.");
            }

            return transactions;
        }

        public async Task<TransactionApiModel> GetTransactionAsync(int transactionId)
        {
            var response = await _httpClient.GetAsync($"api/transactionsapi/{transactionId}");
            response.EnsureSuccessStatusCode();
            var transactionApiModel = await response.Content.ReadFromJsonAsync<TransactionApiModel>();

            if (transactionApiModel == null)
            {
                throw new InvalidOperationException($"Transaction with ID {transactionId} could not be retrieved.");
            }

            return transactionApiModel;
        }

        public async Task<bool> RecordTransactionAsync(TransactionApiModel transaction)
        {
            var response = await _httpClient.PostAsJsonAsync("api/transactionsapi", transaction);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId)
        {
            var response = await _httpClient.DeleteAsync($"api/transactionsapi/{transactionId}");
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}
