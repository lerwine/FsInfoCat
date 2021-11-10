using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a background operation that produces a result value.
    /// </summary>
    public interface IBgProducer<TResult> : IBgOperation
    {
        /// <summary>
        /// Gets the <see cref="Task{TResult}"/> for the background operation.
        /// </summary>
        /// <remarks>If <see cref="Task.Status"/> is <see cref="TaskStatus.Running"/>, this does not necessarily indicate that the background operation is in progress. Refer to <see cref="Status"/> to get the status of the background operation.</remarks>
        new Task<TResult> Task { get; }
    }

    public interface IBgProducer<TState, TResult> : IBgProducer<TResult>, IBgOperation<TState> { }
}
