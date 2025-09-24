using ItemManagement.Domain.Entities;
using ItemManagement.Infrastructure.Data;
using ItemManagement.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ItemManagement.Infrastructure.Tests.Repository
{
    public class UnitOfWorkTests 
    {
        private readonly Mock<ApplicationDbContext> _dbContextMock;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _unitOfWork = new UnitOfWork(_dbContextMock.Object);
        }

        [Fact]
        public void Save_Calls_DbContextSaveChanges()
        {
            _dbContextMock.Setup(db => db.SaveChanges()).Returns(1);

            _unitOfWork.Save();

            _dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task SaveAsync_Calls_DbContextSaveChangesAsync()
        {
            _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _unitOfWork.SaveAsync();

            Assert.Equal(1, result);
            _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task BeginTransactionAsync_StartsAndCachesTransaction()
        {
            var dbFacadeMock = new Mock<DatabaseFacade>(_dbContextMock.Object);
            _dbContextMock.SetupGet(db => db.Database).Returns(dbFacadeMock.Object);

            var transactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(transactionMock.Object);

            var tx1 = await _unitOfWork.BeginTransactionAsync();
            var tx2 = await _unitOfWork.BeginTransactionAsync();

            Assert.Same(tx1, tx2);
            dbFacadeMock.Verify(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CommitTransactionAsync_CommitsAndDisposes()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            SetPrivateTransaction(transactionMock.Object);

            await _unitOfWork.CommitTransactionAsync();

            transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            transactionMock.Verify(t => t.DisposeAsync(), Times.Once);
        }

        [Fact]
        public async Task RollbackTransactionAsync_RollbacksAndDisposes()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            SetPrivateTransaction(transactionMock.Object);

            await _unitOfWork.RollbackTransactionAsync();

            transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
            transactionMock.Verify(t => t.DisposeAsync(), Times.Once);
        }

        private void SetPrivateTransaction(IDbContextTransaction transaction)
        {
            var field = typeof(UnitOfWork).GetField("_transaction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(_unitOfWork, transaction);
        }
 
    }
}
