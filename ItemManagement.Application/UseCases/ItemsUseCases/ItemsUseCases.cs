
using AutoMapper;
using ItemManagement.Domain.Entities;
using ItemManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks; 
 
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.Application.Common.DTO;

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


        public async Task AddItemUseCase(ItemDto dto)
        {
            var item = _mapper.Map<Item>(dto);
            _unitOfWork.Item.AddItem(item);
            await _unitOfWork.SaveAsync();

        }

        public async  Task<bool> DeleteItemUseCase(int ItemId)
        {
            //add checking 
            bool result =  _unitOfWork.Item.DeleteItem(ItemId);
             
            await _unitOfWork.SaveAsync();

            return result;
        }

        public IEnumerable<ItemDto> GetItemsUseCase()
        {
            // return _unitOfWork.Item.GetItems();
            //return _unitOfWork.Item.GetAll(includeProperties: "Category");
            var items = _unitOfWork.Item.GetAll(includeProperties: "Category");
            return items.Select(item => _mapper.Map<ItemDto>(item)).ToList(); 
        }

        public async Task<ItemDto>  GetItemByIdUseCase(int ItemId)
        {
            //return _unitOfWork.Item.GetItemById(ItemId);
            var item = await _unitOfWork.Item.GetItemByIdAsync(ItemId);
            return item == null ? null : _mapper.Map<ItemDto>(item);
        }

        public async Task<bool> EditItemUseCase(ItemDto dto)
        {
            var item = await  _unitOfWork.Item.GetItemByIdAsync(dto.ItemId);
            if (item == null)
            {
                return false;  // signal "item not found"
            }
            _mapper.Map(dto, item);
            _unitOfWork.Item.UpdateItem(item);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public bool CheckItemExists(string name)
        {
            return _unitOfWork.Item.Any(u => u.Name == name);
        }
        public IEnumerable<ItemDto> ViewItemsByCategoryId(int categoryId)
        {
            // return itemRepository.GetItemsByCategoryId(categoryId);
            var items = _unitOfWork.Item.GetItemsByCategoryId(categoryId);
            var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);
            return itemDtos;
        }
    }
}
