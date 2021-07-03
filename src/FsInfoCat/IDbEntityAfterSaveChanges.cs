using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IDbEntityAfterSaveChanges
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked after changes to the current entity have been updated in the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous post-update activity implemented by the this callback method.</returns>
        Task AfterSaveChangesAsync(CancellationToken cancellationToken);
    }
}
