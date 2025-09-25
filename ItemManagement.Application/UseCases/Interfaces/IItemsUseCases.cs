using ItemManagement.Application.Common.DTO;
using ItemManagement.Domain.Entities;
using ItemManagement.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.Application.UseCaseInterfaces
{
    public interface IItemsUseCases
    {
        Task<IEnumerable<ItemDto>> GetItemsUseCase(string userId, CancellationToken cancellationToken = default);
        Task<ItemDto> GetItemByIdUseCase(int itemId, string userId, CancellationToken cancellationToken = default);
        Task AddItemUseCase(ItemDto itemDto, string userId, CancellationToken cancellationToken = default);
        Task<bool> EditItemUseCase(ItemDto itemDto, string userId, CancellationToken cancellationToken = default);
        Task<bool> DeleteItemUseCase(int itemId, string userId, CancellationToken cancellationToken = default);

        IEnumerable<ItemDto> ViewItemsByCategoryId(int categoryId);
    }
}
