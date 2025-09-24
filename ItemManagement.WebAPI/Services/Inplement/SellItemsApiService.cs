using ItemManagement.WebAPI.Models;
using ItemManagement.WebAPI.Services.Interfaces;

namespace ItemManagement.WebAPI.Services.Inplement
{
    public class SellItemsApiService: ISellItemsApiService
    {
        private readonly HttpClient _httpClient;

        public SellItemsApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateSellOrderAsync(SellOrderApiModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/SellItemsApi/Create", model);
            return response.IsSuccessStatusCode;
        }
    }

}