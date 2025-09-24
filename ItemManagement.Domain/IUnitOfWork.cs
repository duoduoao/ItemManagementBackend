using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository Item  { get; }
        ICategoryRepository Category  { get; }
        ITransactionRepository Transaction { get; }
 
        void Save();
        Task<IDbContextTransaction> BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
        Task<int> SaveAsync();
    }
}
