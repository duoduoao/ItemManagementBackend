using AutoMapper;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Application.UseCases;
using ItemManagement.Domain.Entities;
using ItemManagement.Domain.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class ItemsUseCasesTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ItemsUseCases _itemsUseCases;

    public ItemsUseCasesTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _itemsUseCases = new ItemsUseCases(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task AddItemUseCase_Should_Map_And_Add_Item_Then_Save()
    {
        var dto = new ItemDto { ItemId = 1 };
        var itemEntity = new Item { ItemId = 1 };

        _mapperMock.Setup(m => m.Map<Item>(dto)).Returns(itemEntity);
        _unitOfWorkMock.Setup(u => u.Item.AddItemAsync(itemEntity, It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

        await _itemsUseCases.AddItemUseCase(dto, "user1");

        _mapperMock.Verify(m => m.Map<Item>(dto), Times.Once);
        _unitOfWorkMock.Verify(u => u.Item.AddItemAsync(itemEntity, "user1", It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteItemUseCase_Should_Call_Delete_And_Save_ReturnsTrue()
    {
        _unitOfWorkMock.Setup(u => u.Item.DeleteItemAsync(1, "user1", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

        var result = await _itemsUseCases.DeleteItemUseCase(1, "user1");

        Assert.True(result);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetItemsUseCase_Should_Return_Mapped_ItemDtos()
    {
        var items = new List<Item> { new Item { ItemId = 1 }, new Item { ItemId = 2 } };
        _unitOfWorkMock.Setup(u => u.Item.GetItemsAsync("user1", It.IsAny<CancellationToken>())).ReturnsAsync(items);

        _mapperMock.Setup(m => m.Map<ItemDto>(items[0])).Returns(new ItemDto { ItemId = 1 });
        _mapperMock.Setup(m => m.Map<ItemDto>(items[1])).Returns(new ItemDto { ItemId = 2 });

        var result = await _itemsUseCases.GetItemsUseCase("user1");

        Assert.Collection(result,
            dto => Assert.Equal(1, dto.ItemId),
            dto => Assert.Equal(2, dto.ItemId)
        );
    }

    [Fact]
    public async Task GetItemByIdUseCase_Should_Return_Mapped_ItemDto_Or_Null()
    {
        var item = new Item { ItemId = 1 };
        var dto = new ItemDto { ItemId = 1 };

        _unitOfWorkMock.Setup(u => u.Item.GetItemByIdAsync(1, "user1", It.IsAny<CancellationToken>())).ReturnsAsync(item);
        _mapperMock.Setup(m => m.Map<ItemDto>(item)).Returns(dto);

        var result = await _itemsUseCases.GetItemByIdUseCase(1, "user1");
        Assert.NotNull(result);
        Assert.Equal(1, result.ItemId);

        _unitOfWorkMock.Setup(u => u.Item.GetItemByIdAsync(2, "user1", It.IsAny<CancellationToken>())).ReturnsAsync((Item)null);
        result = await _itemsUseCases.GetItemByIdUseCase(2, "user1");
        Assert.Null(result);
    }

    [Fact]
    public async Task EditItemUseCase_Should_Update_And_Save_ReturnsTrue()
    {
        var item = new Item { ItemId = 1 };
        var dto = new ItemDto { ItemId = 1 };

        _unitOfWorkMock.Setup(u => u.Item.GetItemByIdAsync(1, "user1", It.IsAny<CancellationToken>())).ReturnsAsync(item);
        _unitOfWorkMock.Setup(u => u.Item.UpdateItemAsync(item, "user1", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

        var result = await _itemsUseCases.EditItemUseCase(dto, "user1");

        _mapperMock.Verify(m => m.Map(dto, item), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task EditItemUseCase_Should_ReturnFalse_If_Item_NotFound()
    {
        var dto = new ItemDto { ItemId = 99 };
        _unitOfWorkMock.Setup(u => u.Item.GetItemByIdAsync(99, "user1", It.IsAny<CancellationToken>())).ReturnsAsync((Item)null);

        var result = await _itemsUseCases.EditItemUseCase(dto, "user1");

        Assert.False(result);
    }

    [Fact]
    public void CheckItemExists_Should_Call_Any_On_UnitOfWork()
    {
        _unitOfWorkMock.Setup(u => u.Item.Any(It.IsAny<System.Linq.Expressions.Expression<System.Func<Item, bool>>>())).Returns(true);

        var exists = _itemsUseCases.CheckItemExists("name");

        Assert.True(exists);
        _unitOfWorkMock.Verify(u => u.Item.Any(It.IsAny<System.Linq.Expressions.Expression<System.Func<Item, bool>>>()), Times.Once);
    }

    [Fact]
    public void ViewItemsByCategoryId_Should_Return_Mapped_Dtos()
    {
        var items = new List<Item> { new Item { ItemId = 1 }, new Item { ItemId = 2 } };
        var dtos = new List<ItemDto> { new ItemDto { ItemId = 1 }, new ItemDto { ItemId = 2 } };

        _unitOfWorkMock.Setup(u => u.Item.GetItemsByCategoryId(5)).Returns(items.AsQueryable());
        _mapperMock.Setup(m => m.Map<IEnumerable<ItemDto>>(items)).Returns(dtos);

        var result = _itemsUseCases.ViewItemsByCategoryId(5);

        Assert.Equal(2, result.Count());
    }
}
