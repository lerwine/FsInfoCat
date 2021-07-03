using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Interface database entity objects that will get notified before being inserted or updated into the database.
    /// </summary>
    public interface IDbEntityBeforeSave
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked before the current entity is inserted or updated into the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous before-save activity implemented by the this callback method.</returns>
        Task BeforeSaveAsync(CancellationToken cancellationToken);
    }
}
