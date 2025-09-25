using AutoMapper;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.Domain.Entities;
using ItemManagement.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.Application.UseCases
{
    public class ItemsUseCases : IItemsUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemsUseCases(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddItemUseCase(ItemDto dto, string userId, CancellationToken cancellationToken = default)
        {
            var item = _mapper.Map<Item>(dto);
            await _unitOfWork.Item.AddItemAsync(item, userId, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public async Task<bool> DeleteItemUseCase(int itemId, string userId, CancellationToken cancellationToken = default)
        {
            bool result = await _unitOfWork.Item.DeleteItemAsync(itemId, userId, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            return result;
        }

        public async Task<IEnumerable<ItemDto>> GetItemsUseCase(string userId, CancellationToken cancellationToken = default)
        {
            var items = await _unitOfWork.Item.GetItemsAsync(userId, cancellationToken);
            return items.Select(item => _mapper.Map<ItemDto>(item)).ToList();
        }

        public async Task<ItemDto> GetItemByIdUseCase(int itemId, string userId, CancellationToken cancellationToken = default)
        {
            var item = await _unitOfWork.Item.GetItemByIdAsync(itemId, userId, cancellationToken);
            return item == null ? null : _mapper.Map<ItemDto>(item);
        }

        public async Task<bool> EditItemUseCase(ItemDto dto, string userId, CancellationToken cancellationToken = default)
        {
            var item = await _unitOfWork.Item.GetItemByIdAsync(dto.ItemId, userId, cancellationToken);
            if (item == null)
            {
                return false;
            }
            _mapper.Map(dto, item);
            await _unitOfWork.Item.UpdateItemAsync(item, userId, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            return true;
        }

        public bool CheckItemExists(string name)
        {
            return _unitOfWork.Item.Any(u => u.Name == name);
        }

        public IEnumerable<ItemDto> ViewItemsByCategoryId(int categoryId)
        {
            var items = _unitOfWork.Item.GetItemsByCategoryId(categoryId);
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }
    }
}
