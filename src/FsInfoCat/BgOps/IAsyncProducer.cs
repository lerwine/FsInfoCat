using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    /// <summary>
    /// Represents an asynchronous operation that produces a result value.
    /// </summary>
    /// <typeparam name="TResult">The result value type.</typeparam>
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncProducer<TResult> : ICustomAsyncProducer<IAsyncOpEventArgs, TResult>, IAsyncOperation
    {
    }

    /// <summary>
    /// Represents an asynchronous operation that produces a result value.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
    /// <typeparam name="TResult">The result value type.</typeparam>
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncProducer<TState, TResult> : IAsyncProducer<TResult>, ICustomAsyncProducer<TState, AsyncOpEventArgs<TState>, TResult>, IAsyncOperation<TState>
    {
    }
}
