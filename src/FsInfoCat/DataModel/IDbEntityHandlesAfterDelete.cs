using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database entity objects that get notified by <see cref="BaseDbContext"/> when
    /// its <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State"/> is <see cref="Microsoft.EntityFrameworkCore.EntityState.Deleted"/>
    /// and it has been deleted from the database.
    /// </summary>
    [System.Obsolete("Use FsInfoCat.Model.IDbEntityHandlesAfterDelete")]
    public interface IDbEntityHandlesAfterDelete
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked after the current entity has been deleted from the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous post-delete activity implemented by the this callback method.</returns>
        Task AfterDeleteAsync(CancellationToken cancellationToken);
    }
}
