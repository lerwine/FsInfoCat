using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for database entity objects that get notified by <see cref="BaseDbContext"/> when
    /// its <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State"/> is <see cref="Microsoft.EntityFrameworkCore.EntityState.Added"/>
    /// or <see cref="Microsoft.EntityFrameworkCore.EntityState.Modified"/> and the changes are about to be updated or inserted to the database.
    /// </summary>
    [System.Obsolete("Use FsInfoCat.Model.IDbEntityHandlesBeforeSave")]
    public interface IDbEntityHandlesBeforeSave
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked before the current entity is about to be updated or inserted into the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous before-save activity implemented by the this callback method.</returns>
        Task BeforeSaveAsync(CancellationToken cancellationToken);
    }
}
