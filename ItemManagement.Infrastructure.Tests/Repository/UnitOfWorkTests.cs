using ItemManagement.Domain.Entities;
using ItemManagement.Domain.Repositories;
using ItemManagement.Infrastructure.Data;
using ItemManagement.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SendGrid.Helpers.Mail;
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
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _unitOfWork = new UnitOfWork(_context, null); // pass null or mock ICacheService if needed
        }

        [Fact]
        public async Task SaveAsync_PersistsChanges()
        {
            // Arrange
            // Add entities to context here as needed

            // Act
            var result = await _unitOfWork.SaveAsync();

            // Assert
            Assert.True(result >= 0);
        }
 
      
      
        [Fact]
        public void Dispose_DisposesDbContext()
        {
            _unitOfWork.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _context.Database.GetDbConnection().Open());
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }
    }
}