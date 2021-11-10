using System;
using System.Threading.Tasks;

namespace FsInfoCat.Services
{

    /// <summary>
    /// Represents a background operation that produces a result value, enqueued by the <see cref="IBgOperationQueue"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the background operation.</typeparam>
    /// <seealso cref="IBgOperationQueue.Enqueue{TProgress, TResult}(ActivityCode, Func{IQueuedBgOperation, TProgress}, Func{TProgress, Task{TResult}})" />
    /// <seealso cref="IBgOperationQueue.Enqueue{T}(ActivityCode, Func{IAsyncOperationProgress, Task{T}})" />
    /// <seealso cref="IBgOperationQueue.Enqueue{T}(ActivityCode, Func{System.Threading.CancellationToken, Task{T}})" />
    public interface IQueuedBgProducer<TResult> : IQueuedBgOperation, IBgProducer<TResult> { }

    /// <summary>>
    /// Represents a background operation that produces a result value, enqueued by the <see cref="IBgOperationQueue"/>.
    /// </summary>
    /// <typeparam name="TState">The type of the t state.</typeparam>
    /// <typeparam name="TResult">The type of the t result.</typeparam>
    /// <seealso cref="IBgOperationQueue.Enqueue{TState, TProgress, TResult}(ActivityCode, TState, Func{TState, IQueuedBgOperation, TProgress}, Func{TState, TProgress, Task{TResult}})" />
    /// <seealso cref="IBgOperationQueue.Enqueue{TState, TResult}(ActivityCode, TState, Func{TState, IAsyncOperationProgress, Task{TResult}})" />
    /// <seealso cref="IBgOperationQueue.Enqueue{TState, TResult}(ActivityCode, TState, Func{TState, System.Threading.CancellationToken, Task{TResult}})" />
    public interface IQueuedBgProducer<TState, TResult> : IQueuedBgProducer<TResult>, IQueuedBgOperation<TState>, IBgProducer<TState, TResult> { }
}
