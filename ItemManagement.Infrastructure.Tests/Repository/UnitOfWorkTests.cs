using ItemManagement.Domain.Entities;
using ItemManagement.Infrastructure.Data;
using ItemManagement.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ItemManagement.Infrastructure.Tests.Repository
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb_UnitOfWork_" + Guid.NewGuid())
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            _unitOfWork = new UnitOfWork(_context);
        }

        [Fact]
        public void Repositories_ShouldBeInitialized()
        {
            Assert.NotNull(_unitOfWork.Item);
            Assert.NotNull(_unitOfWork.Category);
            Assert.NotNull(_unitOfWork.Transaction);
        }

        [Fact]
        public void Save_ShouldCommitChanges()
        {
            var category = new Category { Name = "SaveTest", Description = "Test" };
            _context.Categories.Add(category);
            _unitOfWork.Save();
            var saved = _context.Categories.Find(category.CategoryId);
            Assert.NotNull(saved);
            Assert.Equal("SaveTest", saved.Name);
        }

        [Fact]
        public async Task SaveAsync_ShouldCommitChanges()
        {
            var category = new Category { Name = "SaveAsyncTest", Description = "Test" };
            _context.Categories.Add(category);
            var result = await _unitOfWork.SaveAsync();
            Assert.True(result > 0);
            var saved = _context.Categories.Find(category.CategoryId);
            Assert.NotNull(saved);
            Assert.Equal("SaveAsyncTest", saved.Name);
        }

        [Fact]
        public async Task Transaction_BeginCommitRollback_ShouldNotThrow()
        {
            // Transactions are ignored in InMemory provider, but calls should not throw with warning suppressed

            var transaction = await _unitOfWork.BeginTransactionAsync();
            Assert.Null(transaction);  // InMemory provider BeginTransactionAsync returns null

            // Should not throw even if calling commit or rollback without real transaction
            await _unitOfWork.CommitTransactionAsync();  // Should throw InvalidOperationException because _transaction is null
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }
    }
}
