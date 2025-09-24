using AutoMapper;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.Application.UseCasesInterfaces;
using ItemManagement.Application.UserCases;
using ItemManagement.Domain.Entities;
using ItemManagement.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ItemManagement.Application.Tests.UseCases
{
    public class TransactionsUseCasesTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IItemsUseCases> _itemsUseCasesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TransactionsUseCases _useCases;

        public TransactionsUseCasesTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _itemsUseCasesMock = new Mock<IItemsUseCases>();
            _mapperMock = new Mock<IMapper>();

            _useCases = new TransactionsUseCases(
                _unitOfWorkMock.Object,
                _itemsUseCasesMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public void GetTodayTransactions_ShouldReturnMappedDtos()
        {
            // Arrange
            var domainTransactions = new List<Transaction>
            {
                new Transaction { TransactionId = 1, CashierName = "Cashier1", ItemName = "Test", SoldQty = 5, TimeStamp = DateTime.Now }
            };

            var mapped = new List<TransactionDto>
            {
                new TransactionDto { TransactionId = 1, CashierName = "Cashier1", ItemName = "Test", SoldQty = 5 }
            };

            _unitOfWorkMock.Setup(u => u.Transaction.GetByDay("Cashier1", It.IsAny<DateTime>()))
                           .Returns(domainTransactions);
            _mapperMock.Setup(m => m.Map<IEnumerable<TransactionDto>>(domainTransactions))
                       .Returns(mapped);

            // Act
            var result = _useCases.GetTodayTransactions("Cashier1");

            // Assert
            Assert.Single(result);
            Assert.Equal("Test", result.First().ItemName);
        }

        [Fact]
        public async Task RecordTransactionAsync_ShouldCallSaveAndCommit()
        {
            // Arrange
            // Arrange
            var itemDto = new ItemDto
            {
                Name = "Item1",
                Price = 10.0,
                Quantity = 100,
            };
            _itemsUseCasesMock
      .Setup(m => m.GetItemByIdUseCase(It.IsAny<int>()))
      .ReturnsAsync(itemDto);


            // Act
            await _useCases.RecordTransactionAsync("Cashier2", 1, 5);

            // Assert
            _unitOfWorkMock.Verify(u => u.Transaction.Save("Cashier2", 1, "Item1", 10.0, 100, 5), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public void GetTransactions_ShouldMapFromRepository()
        {
            // Arrange
            var domainTransactions = new List<Transaction>
            {
                new Transaction { TransactionId = 10, CashierName = "Cashier3", ItemName = "Sample", SoldQty = 2 }
            };

            var mapped = new List<TransactionDto>
            {
                new TransactionDto { TransactionId = 10, CashierName = "Cashier3", ItemName = "Sample", SoldQty = 2 }
            };

            _unitOfWorkMock.Setup(u => u.Transaction.Search("Cashier3", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                           .Returns(domainTransactions);
            _mapperMock.Setup(m => m.Map<IEnumerable<TransactionDto>>(domainTransactions))
                       .Returns(mapped);

            // Act
            var result = _useCases.GetTransactions("Cashier3", DateTime.Now.AddDays(-1), DateTime.Now);

            // Assert
            Assert.Single(result);
            Assert.Equal("Sample", result.First().ItemName);
        }

        [Fact]
        public void GetAllTransactions_ShouldReturnAll()
        {
            // Arrange
            var domainTransactions = new List<Transaction>
            {
                new Transaction { TransactionId = 20, CashierName = "Cashier4", ItemName = "AllItem", SoldQty = 7 }
            };

            var mapped = new List<TransactionDto>
            {
                new TransactionDto { TransactionId = 20, CashierName = "Cashier4", ItemName = "AllItem", SoldQty = 7 }
            };

            _unitOfWorkMock.Setup(u => u.Transaction.GetAll()).Returns(domainTransactions);
            _mapperMock.Setup(m => m.Map<IEnumerable<TransactionDto>>(domainTransactions))
                       .Returns(mapped);

            // Act
            var result = _useCases.GetAllTransactions();

            // Assert
            Assert.Single(result);
            Assert.Equal("AllItem", result.First().ItemName);
        }
    }
}
