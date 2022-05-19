using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database entity objects that get notified by <see cref="BaseDbContext"/> when
    /// its <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State"/> was <see cref="Microsoft.EntityFrameworkCore.EntityState.Modified"/>
    /// and it has been inserted or updated into the database.
    /// </summary>
    [System.Obsolete("Use FsInfoCat.Model.IDbEntityHandlesAfterSaveChanges")]
    public interface IDbEntityHandlesAfterSaveChanges
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked after changes to the current entity have been updated in the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous post-update activity implemented by the this callback method.</returns>
        Task AfterSaveChangesAsync(CancellationToken cancellationToken);
    }
}
