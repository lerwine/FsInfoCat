using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IDbEntityBeforeDelete
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked before the current entity is deleted from the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous pre-delete activity implemented by the this callback method.</returns>
        Task BeforeDeleteAsync(CancellationToken cancellationToken);
    }
}
