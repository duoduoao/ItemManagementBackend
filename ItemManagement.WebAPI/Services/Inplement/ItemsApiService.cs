using ItemManagement.WebAPI.Models;
using ItemManagement.WebAPI.Services.Interfaces;
using System.Net;
using System.Net.Http;
namespace ItemManagement.WebAPI.Services.Inplement
{
    public class ItemsApiService : IItemsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ItemsApiService> _logger;
        public ItemsApiService(HttpClient httpClient, ILogger<ItemsApiService> logger)
        {
            _httpClient = httpClient;
            _logger  = logger;
        }

        public async Task<IEnumerable<ItemApiModel>> GetItemsAsync()
        {
            var response = await _httpClient.GetAsync("api/itemsapi");
            response.EnsureSuccessStatusCode();
            IEnumerable<ItemApiModel>? enumerable = await response.Content.ReadFromJsonAsync<IEnumerable<ItemApiModel>>();
            return enumerable ?? Enumerable.Empty<ItemApiModel>();
        }

        public async Task<ItemApiModel> GetItemAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/itemsapi/{id}");
            response.EnsureSuccessStatusCode();
            var item = await response.Content.ReadFromJsonAsync<ItemApiModel>();
            if (item == null)
            {
                throw new InvalidOperationException("The API returned a null item.");
            }
            return item;
        }


        public async Task<bool> UpdateItemAsync(ItemApiModel item)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/itemsapi/{item.ItemId}", item);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddItemAsync(ItemApiModel item)
        {
            var response = await _httpClient.PostAsJsonAsync("api/itemsapi", item);
            //response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            { 
                _logger.LogInformation("BadRequest item with ID {ItemId}", item.ItemId);

                // Optionally read validation error details from response.Content
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log or handle errorContent as needed for debugging or UI feedback
                return false;
            }
            else
            {
                // Handle other error status codes as needed
                return false;
            }
            //return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/itemsapi/{id}");
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }


        public async Task<IEnumerable<ItemApiModel>> GetItemsByCategoryAsync(int categoryId)
        {
            // Call your backend API endpoint or DB query to get filtered items
            var response = await _httpClient.GetAsync($"api/itemsapi/category/{categoryId}");
            response.EnsureSuccessStatusCode();

            var items = await response.Content.ReadFromJsonAsync<IEnumerable<ItemApiModel>>();
            return items ?? Enumerable.Empty<ItemApiModel>();
        }

    }
}
