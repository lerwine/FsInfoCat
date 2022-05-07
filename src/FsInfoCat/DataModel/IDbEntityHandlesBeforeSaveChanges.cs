using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database entity objects that get notified by <see cref="BaseDbContext"/> when
    /// its <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State"/> is <see cref="Microsoft.EntityFrameworkCore.EntityState.Modified"/>
    /// and the changes are about to be saved to the database.
    /// </summary>
    public interface IDbEntityHandlesBeforeSaveChanges
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked when the <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State"/> value
        /// is <see cref="Microsoft.EntityFrameworkCore.EntityState.Modified"/> and the changes are about to be saved to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous pre-update activity implemented by the this callback method.</returns>
        Task BeforeSaveChangesAsync(CancellationToken cancellationToken);
    }
}
