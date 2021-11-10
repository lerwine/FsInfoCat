namespace FsInfoCat.Services
{
    /// <summary>
    /// Represents a background operation enqueued by the <see cref="IBgOperationQueue"/>.
    /// </summary>
    /// <seealso cref="IBgOperationQueue.Enqueue(ActivityCode, System.Func{IAsyncOperationProgress, System.Threading.Tasks.Task})" />
    /// <seealso cref="IBgOperationQueue.Enqueue(ActivityCode, System.Func{System.Threading.CancellationToken, System.Threading.Tasks.Task})" />
    /// <seealso cref="IBgOperationQueue.Enqueue{T}(ActivityCode, System.Func{IQueuedBgOperation, T}, System.Func{T, System.Threading.Tasks.Task})" />
    public interface IQueuedBgOperation : IBgOperationEventArgs, IBgOperation { }

    /// <summary>
    /// Represents a background operation enqueued by the <see cref="IBgOperationQueue"/>.
    /// </summary>
    /// <typeparam name="T">The type of the asynchronous state value.</typeparam>
    /// <seealso cref="IBgOperationQueue.Enqueue{T}(ActivityCode, T, System.Func{T, IAsyncOperationProgress, System.Threading.Tasks.Task})" />
    /// <seealso cref="IBgOperationQueue.Enqueue{T}(ActivityCode, T, System.Func{T, System.Threading.CancellationToken, System.Threading.Tasks.Task})" />
    /// <seealso cref="IBgOperationQueue.Enqueue{TState, TProgress}(ActivityCode, TState, System.Func{TState, IQueuedBgOperation, TProgress}, System.Func{TState, TProgress, System.Threading.Tasks.Task})" />
    public interface IQueuedBgOperation<T> : IQueuedBgOperation, IBgOperationEventArgs<T>, IBgOperation<T> { }
}
