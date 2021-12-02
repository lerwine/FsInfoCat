using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncFunc<TResult> : IAsyncAction
    {
        /// <summary>
        /// Gets the <see cref="Task{TResult}"/> that produces the result value for the asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        new Task<TResult> Task { get; }
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncFunc<TState, TResult> : IAsyncFunc<TResult>, IAsyncAction<TState>
    {
    }
}
