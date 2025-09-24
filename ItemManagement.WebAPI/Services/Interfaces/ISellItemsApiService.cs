using ItemManagement.WebAPI.Models;

namespace ItemManagement.WebAPI.Services.Interfaces
{
    public interface ISellItemsApiService
    {
        Task<bool> CreateSellOrderAsync(SellOrderApiModel model);
    }
}
