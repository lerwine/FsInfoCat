using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IDbEntityBeforeSaveChanges
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked before changes to the current entity are updated in the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous pre-update activity implemented by the this callback method.</returns>
        Task BeforeSaveChangesAsync(CancellationToken cancellationToken);
    }
}
