using System;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{
    /// <summary>
    /// Represents a background operation enqueued by the <see cref="IFSIOQueueService"/>.
    /// </summary>
    public interface IQueuedBgOperation : IBgOperationEventArgs, IBgOperation
    {
    }

    /// <summary>
    /// Represents a background operation that produces a result value, enqueued by the <see cref="IFSIOQueueService"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
    public interface IQueuedBgOperation<TResult> : IQueuedBgOperation, IBgOperation<TResult>
    {
    }
}
