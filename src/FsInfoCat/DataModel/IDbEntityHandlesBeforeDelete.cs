using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database entity objects that get notified by <see cref="BaseDbContext"/> when
    /// its <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State"/> is <see cref="Microsoft.EntityFrameworkCore.EntityState.Deleted"/>
    /// and it is about to be deleted from database.
    /// </summary>
    public interface IDbEntityHandlesBeforeDelete
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked before the current entity is deleted from the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous pre-delete activity implemented by the this callback method.</returns>
        Task BeforeDeleteAsync(CancellationToken cancellationToken);
    }
}
