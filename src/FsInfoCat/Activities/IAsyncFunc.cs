using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an asynchronous activity that produces a result value.
    /// </summary>
    /// <typeparam name="TEvent">The base type of the push-notification events raised by this activity.</typeparam>
    /// <typeparam name="TResult">The type of the result value produced by this asynchronous function.</typeparam>
    /// <seealso cref="IAsyncAction{TEvent}" />
    public interface IAsyncFunc<TEvent, TResult> : IAsyncAction<TEvent>
         where TEvent : IOperationEvent
    {
        /// <summary>
        /// Gets the task for the asynchronous function.
        /// </summary>
        /// <value>The <see cref="Task{TResult}"/> that asynchronously produces the result value.</value>
        new Task<TResult> Task { get; }
    }

    /// <summary>
    /// Represents an asynchronous activity that is associated with a user-specified value and produces a result value.
    /// </summary>
    /// <typeparam name="TEvent">The base type of the push-notification events raised by this activity.</typeparam>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this asynchronous function.</typeparam>
    /// <typeparam name="TResult">The type of the result value produced by this asynchronous function.</typeparam>
    /// <seealso cref="IAsyncAction{TEvent, TState}" />
    /// <seealso cref="IAsyncFunc{TEvent, TResult}" />
    public interface IAsyncFunc<TEvent, TState, TResult> : IAsyncAction<TEvent, TState>, IAsyncFunc<TEvent, TResult> where TEvent : IOperationEvent<TState> { }
}
