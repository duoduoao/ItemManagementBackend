using ItemManagement.Application.Common.DTO;
using ItemManagement.WebAPI.Models;
using ItemManagement.WebAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ItemManagement.WebAPI.Services.Implement
{
    public class CategoriesApiService : ICategoriesApiService
    {
        private readonly HttpClient _httpClient;

        public CategoriesApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CategoryApiModel>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("api/categoriesapi");
            response.EnsureSuccessStatusCode();
            var enumerableCat = await response.Content.ReadFromJsonAsync<IEnumerable<CategoryApiModel>>();
            return enumerableCat ?? new List<CategoryApiModel>();
        }

        public async Task<CategoryApiModel> GetCategoryAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/categoriesapi/{id}");
            response.EnsureSuccessStatusCode();
            var category = await response.Content.ReadFromJsonAsync<CategoryApiModel>();
            if (category == null)
            {
                throw new InvalidOperationException($"Category with ID {id} was not found.");
            }
            return category;
        }

        public async Task<bool> AddCategoryAsync(CategoryApiModel category)
        {
            var response = await _httpClient.PostAsJsonAsync("api/categoriesapi", category);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryApiModel category)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/categoriesapi/{category.CategoryId}", category);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteCategoryAsync(CategoryApiModel category)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/categoriesapi");
            var json = JsonSerializer.Serialize(category);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var errorJson = await response.Content.ReadAsStringAsync();

                var errorObj = JsonSerializer.Deserialize<ErrorResponseDto>(errorJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return (false, errorObj?.Message ?? "Conflict error occurred.");
            }

            response.EnsureSuccessStatusCode();

            return (true, null);
        }
    }
}
