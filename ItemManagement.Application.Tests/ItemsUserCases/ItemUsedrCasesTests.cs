using AutoMapper;
using AutoMapper;
using ItemManagement.Application.Common;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.Application.UseCases;
using ItemManagement.Application.UseCases;
using ItemManagement.Domain.Entities;
using ItemManagement.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
namespace ItemManagement.Application.Tests
{

    public class ItemsUseCasesTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ItemsUseCases _useCases;

        public ItemsUseCasesTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _useCases = new ItemsUseCases(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddItemUseCase_ValidDto_CallsAddAndSave()
        {
            // Arrange
            var dto = new ItemDto { ItemId = 1, Name = "TestItem" };
            var item = new Item();

            _mapperMock.Setup(m => m.Map<Item>(dto)).Returns(item);
            _unitOfWorkMock.SetupGet(u => u.Item).Returns(Mock.Of<IItemRepository>());
            _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            await _useCases.AddItemUseCase(dto);

            // Assert
            Mock.Get(_unitOfWorkMock.Object.Item).Verify(repo => repo.AddItem(item), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteItemUseCase_ItemExists_ReturnsTrueAndSaves()
        {
            // Arrange
            int itemId = 1;
            var itemRepoMock = new Mock<IItemRepository>();
            itemRepoMock.Setup(r => r.DeleteItem(itemId)).Returns(true);
            _unitOfWorkMock.SetupGet(u => u.Item).Returns(itemRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await _useCases.DeleteItemUseCase(itemId);

            // Assert
            Assert.True(result);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }


        //[Fact]
        //public void GetItemsUseCase_ReturnsMappedDtoList()
        //{
        //    // Arrange
        //    var items = new List<Item> { new Item() };

        //    // Convert list to IQueryable for matching return type
        //    var queryableItems = items.AsQueryable();

        //    var itemRepoMock = new Mock<IItemRepository>();

        //    // Setup GetAll with explicit all parameters including optional ones
        //    itemRepoMock.Setup(m => m.GetAll(
        //            It.IsAny<Expression<Func<Item, bool>>>(),  // any filter expression or null
        //            "Category",                                // includeProperties = "Category"
        //            false                                     // tracked = false -- explicitly specified
        //        ))
        //        .Returns(queryableItems);

        //    _unitOfWorkMock.SetupGet(u => u.Item).Returns(itemRepoMock.Object);

        //    // Setup mapper to map each item to ItemDto
        //    _mapperMock.Setup(m => m.Map<ItemDto>(It.IsAny<Item>())).Returns(new ItemDto());

        //    // Act
        //    var results = _useCases.GetItemsUseCase();

        //    // Assert
        //    Assert.NotEmpty(results);

        //    // Verify Map<ItemDto> called exactly as many times as there are items
        //    _mapperMock.Verify(m => m.Map<ItemDto>(It.IsAny<Item>()), Times.Exactly(items.Count));
        //}

        [Fact]
        public async Task GetItemByIdUseCase_ItemExists_ReturnsMappedDto()
        {
            // Arrange
            int itemId = 1;
            var item = new Item();
            _unitOfWorkMock.SetupGet(u => u.Item).Returns(Mock.Of<IItemRepository>());
            Mock.Get(_unitOfWorkMock.Object.Item).Setup(r => r.GetItemByIdAsync(itemId)).ReturnsAsync(item);
            _mapperMock.Setup(m => m.Map<ItemDto>(item)).Returns(new ItemDto());

            // Act
            var result = await _useCases.GetItemByIdUseCase(itemId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EditItemUseCase_ItemExists_UpdatesAndReturnsTrue()
        {
            // Arrange
            var dto = new ItemDto { ItemId = 1, Name = "Updated" };
            var item = new Item();

            _unitOfWorkMock.SetupGet(u => u.Item).Returns(Mock.Of<IItemRepository>());
            Mock.Get(_unitOfWorkMock.Object.Item).Setup(r => r.GetItemByIdAsync(dto.ItemId)).ReturnsAsync(item);
            _mapperMock.Setup(m => m.Map(dto, item));
            _unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await _useCases.EditItemUseCase(dto);

            // Assert
            Assert.True(result);
            Mock.Get(_unitOfWorkMock.Object.Item).Verify(r => r.UpdateItem(item), Times.Once());
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task EditItemUseCase_ItemDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var dto = new ItemDto { ItemId = 1 };
            _unitOfWorkMock.SetupGet(u => u.Item).Returns(Mock.Of<IItemRepository>());
            Mock.Get(_unitOfWorkMock.Object.Item).Setup(r => r.GetItemByIdAsync(dto.ItemId)).ReturnsAsync((Item)null);

            // Act
            var result = await _useCases.EditItemUseCase(dto);

            // Assert
            Assert.False(result);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Never);
        }

        [Fact]
        public void CheckItemExists_ItemExists_ReturnsTrue()
        {
            // Arrange
            var itemRepoMock = new Mock<IItemRepository>();
            itemRepoMock.Setup(r => r.Any(It.IsAny<System.Linq.Expressions.Expression<System.Func<Item, bool>>>())).Returns(true);
            _unitOfWorkMock.SetupGet(u => u.Item).Returns(itemRepoMock.Object);

            // Act
            var exists = _useCases.CheckItemExists("Test");

            // Assert
            Assert.True(exists);
        }
        [Fact]
       
        public void GetItemsUseCase_ReturnsMappedDtoList()
        {
            // Arrange
            var items = new List<Item> { new Item(), new Item(), new Item() };
            var queryableItems = items.AsQueryable();

            var itemRepoMock = new Mock<IItemRepository>();
            // Setup GetAll with explicit parameters including optional ones
            itemRepoMock.Setup(m => m.GetAll(
                    It.IsAny<Expression<Func<Item, bool>>>(),
                    "Category",
                    false
                ))
                .Returns(queryableItems);

            _unitOfWorkMock.SetupGet(u => u.Item).Returns(itemRepoMock.Object);

            // Setup mapper to map each item to ItemDto
            _mapperMock.Setup(m => m.Map<ItemDto>(It.IsAny<Item>()))
                       .Returns(new ItemDto());

            // Act
            var results = _useCases.GetItemsUseCase();

            // Assert
            Assert.NotEmpty(results);
            Assert.Equal(items.Count, results.Count());

            // Verify mapper was called the exact number of times as there are items
            _mapperMock.Verify(m => m.Map<ItemDto>(It.IsAny<Item>()), Times.Exactly(items.Count));
        }


      
    }
}
