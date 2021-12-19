using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an asyncronous activity that does not return a value.
    /// </summary>
    /// <seealso cref="IOperationEvent" />
    public interface IAsyncAction : IOperationEvent
    {
        /// <summary>
        /// Gets the task for the asynchronous operation.
        /// </summary>
        /// <value>The <see cref="Task"/> that executes the asynchronous operation.</value>
        Task Task { get; }
    }

    /// <summary>
    /// Represents an asyncronous activity that does not return a value and is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IOperationEvent{TState}" />
    /// <seealso cref="IAsyncAction" />
    public interface IAsyncAction<TState> : IOperationEvent<TState>, IAsyncAction { }
}
