using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using ItemManagement.WebAPI.Models;

namespace ItemManagement.WebAPI.Services.Interfaces
{
    public interface IItemsApiService
    {
        Task<IEnumerable<ItemApiModel>> GetItemsAsync();
        Task<ItemApiModel> GetItemAsync(int id);
        Task<bool> UpdateItemAsync(ItemApiModel item);
        Task<bool> AddItemAsync(ItemApiModel item);
        Task<bool> DeleteItemAsync(int id);

        Task<IEnumerable<ItemApiModel>> GetItemsByCategoryAsync(int categoryId);

    }
}
