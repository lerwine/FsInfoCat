using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for database entity objects that get notified by <see cref="BaseDbContext"/> when
    /// its <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State"/> was <see cref="Microsoft.EntityFrameworkCore.EntityState.Added"/>
    /// and it has been inserted or updated into the database.
    /// </summary>
    public interface IDbEntityHandlesAfterInsert
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked after the current entity has been inserted into the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous post-insert activity implemented by the this callback method.</returns>
        Task AfterInsertAsync(CancellationToken cancellationToken);
    }
}
