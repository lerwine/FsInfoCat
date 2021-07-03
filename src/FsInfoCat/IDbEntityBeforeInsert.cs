using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IDbEntityBeforeInsert
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked before the current entity is inserted into the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous pre-insert activity implemented by the this callback method.</returns>
        Task BeforeInsertAsync(CancellationToken cancellationToken);
    }
}
