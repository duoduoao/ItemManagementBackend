using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository Item { get; }
        ICategoryRepository Category { get; }
        ITransactionRepository Transaction { get; }

        /// <summary>
        /// Save changes synchronously to the database.
        /// </summary>
        void Save();

        /// <summary>
        /// Save changes asynchronously with support for cancellation.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of affected rows.</returns>
        Task<int> SaveAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins a transaction asynchronously with optional cancellation.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The new database transaction.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the current transaction asynchronously with optional cancellation.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the current transaction asynchronously with optional cancellation.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
