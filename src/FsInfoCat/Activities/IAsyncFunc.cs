using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an asyncronous activity that produces a result value.
    /// </summary>
    /// <typeparam name="TResult">The type of value produced by the activity.</typeparam>
    /// <seealso cref="IAsyncAction" />
    public interface IAsyncFunc<TResult> : IAsyncAction
    {
        /// <summary>
        /// Gets the task for the asynchronous operation.
        /// </summary>
        /// <value>The <see cref="Task{TResult}"/> that asynchronously produces the result value.</value>
        new Task<TResult> Task { get; }
    }

    /// <summary>
    /// Represents an asyncronous activity that produces a result value and is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <typeparam name="TResult">The type of value produced by the activity.</typeparam>
    /// <seealso cref="IAsyncAction{TState}" />
    /// <seealso cref="IAsyncFunc{TResult}" />
    public interface IAsyncFunc<TState, TResult> : IAsyncAction<TState>, IAsyncFunc<TResult> { }
}
