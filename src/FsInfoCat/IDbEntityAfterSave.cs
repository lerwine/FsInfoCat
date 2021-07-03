using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IDbEntityAfterSave
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked after the current entity has been inserted or updated into the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous post-save actity implemented by the this callback method.</returns>
        Task AfterSaveAsync(CancellationToken cancellationToken);
    }
}
