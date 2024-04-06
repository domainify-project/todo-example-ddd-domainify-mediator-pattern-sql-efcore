using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence
{
    /// <summary>
    /// Delegate representing an event that occurs when a database update concurrency conflict is detected.
    /// </summary>
    public delegate void DbUpdateConcurrencyConflictOccurred();

    /// <summary>
    /// Interface for managing database transactions.
    /// </summary>
    public interface IDbTransaction
    {
        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning the database transaction.</returns>
        Task<IDbContextTransaction> BeginAsync();

        /// <summary>
        /// Commits the current database transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CommitAsync();

        /// <summary>
        /// Rolls back the current database transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RollbackAsync();

        /// <summary>
        /// Saves changes to the database asynchronously.
        /// </summary>
        /// <param name="concurrencyCheck">Indicates whether to perform concurrency checks during the save operation.</param>
        /// <param name="toCheckConcurrencyConflictOccurred">
        /// An optional callback to be invoked when a concurrency conflict is detected during the save operation.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation, returning the number of state entries written to the database.
        /// </returns>
        Task<int> SaveChangesAsync(
            bool concurrencyCheck = false,
            DbUpdateConcurrencyConflictOccurred? toCheckConcurrencyConflictOccurred = null);
    }
}