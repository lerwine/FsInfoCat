using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an asynchronous activity.
    /// </summary>
    /// <seealso cref="IOperationInfo" />
    public interface IAsyncActivity : IOperationInfo, IAsyncActivitySource
    {
        /// <summary>
        /// Gets the task for the asyncronous activity.
        /// </summary>
        /// <value>The task for the asyncronous activity.</value>
        Task Task { get; }

        /// <summary>
        /// Gets the token source that can be used to cancel the asyncronous activity.
        /// </summary>
        /// <value>The token source that can be used to cancel the asyncronous activity.</value>
        CancellationTokenSource TokenSource { get; }
    }
}
