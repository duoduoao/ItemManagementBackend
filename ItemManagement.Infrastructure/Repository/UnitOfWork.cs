using ItemManagement.Domain.Repositories;
using ItemManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private IDbContextTransaction _transaction;
        public IItemRepository Item { get; private set;}
        public ICategoryRepository Category { get; private set; } 
 
        public ITransactionRepository Transaction { get; private set;}
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Item = new ItemRepository(_db);
            Category = new CategoryRepository(_db);
            Transaction = new TransactionRepository(_db);   
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_transaction != null)
                return _transaction;

            _transaction = await _db.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No active transaction to commit.");

            await _transaction.CommitAsync();
            await DisposeTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No active transaction to rollback.");

            await _transaction.RollbackAsync();
            await DisposeTransactionAsync();
        }

        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        public void Dispose()
        {
            _db.Dispose();
        }    // Implement the interface method
        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
