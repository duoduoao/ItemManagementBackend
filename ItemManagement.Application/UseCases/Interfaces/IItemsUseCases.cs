
using ItemManagement.Application.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace ItemManagement.Application.UseCaseInterfaces
{
    public interface IItemsUseCases
    {
        IEnumerable<ItemDto> GetItemsUseCase();
        Task<ItemDto> GetItemByIdUseCase(int id);
          Task AddItemUseCase(ItemDto itemDto);
          Task<bool> EditItemUseCase(ItemDto itemDto);
        Task<bool> DeleteItemUseCase(int id);
        IEnumerable<ItemDto> ViewItemsByCategoryId(int categoryId);
    }
}
