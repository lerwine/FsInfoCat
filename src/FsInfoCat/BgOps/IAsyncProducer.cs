using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    /// <summary>
    /// Represents an asynchronous operation that produces a result value.
    /// </summary>
    /// <typeparam name="TResult">The result value type.</typeparam>
    public interface IAsyncProducer<TResult> : IAsyncOperation
    {
        /// <summary>
        /// Gets the <see cref="Task{TResult}"/> that produces the result value for the asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        new Task<TResult> Task { get; }
    }

    /// <summary>
    /// Represents an asynchronous operation that produces a result value.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
    /// <typeparam name="TResult">The result value type.</typeparam>
    public interface IAsyncProducer<TState, TResult> : IAsyncProducer<TResult>, IAsyncOperation<TState>
    {
    }
}
