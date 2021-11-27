using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// A service that can be used to create background jobs that can indicate progress/status information.
    /// </summary>
    [Obsolete("Use Services.IBackgroundProgressService")]
    public interface IAsyncJobFactoryService
    {
        /// <summary>
        /// Starts a new background job that produces a result value.
        /// </summary>
        /// <typeparam name="TArg1">The type of first argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TResult">The type of the value produced by the background job.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg1">The first argument that is passed to the asynchronous job method.</param>
        /// <param name="arg2">The second argument that is passed to the asynchronous job method.</param>
        /// <param name="arg3">The third argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob{TResult}"/> value that can be used to monitor the status of the background job, as well as waiting for the result
        /// value and the ability to cancel the job.</returns>
        IAsyncJob<TResult> StartNew<TArg1, TArg2, TArg3, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
            Func<TArg1, TArg2, TArg3, IStatusListener, Task<TResult>> method);

        /// <summary>
        /// Runs a background job asynchronously that returns a value.
        /// </summary>
        /// <typeparam name="TArg1">The type of first argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TResult">The type of the value produced by the background job.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg1">The first argument that is passed to the asynchronous job method.</param>
        /// <param name="arg2">The second argument that is passed to the asynchronous job method.</param>
        /// <param name="arg3">The third argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <param name="onComplete">The callback to invoke when the background job is complete.</param>
        /// <returns>The task that is executing the background job and returns the result value.</returns>
        Task<TResult> RunAsync<TArg1, TArg2, TArg3, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
            Func<TArg1, TArg2, TArg3, IStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null);

        /// <summary>
        /// Starts a new background job that produces a result value.
        /// </summary>
        /// <typeparam name="TArg1">The type of first argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TResult">The type of the value produced by the background job.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg1">The first argument that is passed to the asynchronous job method.</param>
        /// <param name="arg2">The second argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob{TResult}"/> value that can be used to monitor the status of the background job, as well as waiting for the result
        /// value and the ability to cancel the job.</returns>
        IAsyncJob<TResult> StartNew<TArg1, TArg2, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
            [DisallowNull] Func<TArg1, TArg2, IStatusListener, Task<TResult>> method);

        /// <summary>
        /// Runs a background job asynchronously that returns a value.
        /// </summary>
        /// <typeparam name="TArg1">The type of first argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TResult">The type of the value produced by the background job.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg1">The first argument that is passed to the asynchronous job method.</param>
        /// <param name="arg2">The second argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <param name="onComplete">The callback to invoke when the background job is complete.</param>
        /// <returns>The task that is executing the background job and returns the result value.</returns>
        Task<TResult> RunAsync<TArg1, TArg2, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
            [DisallowNull] Func<TArg1, TArg2, IStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null);

        /// <summary>
        /// Starts a new background job that produces a result value.
        /// </summary>
        /// <typeparam name="TArg">The type of the argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TResult">The type of the value produced by the background job.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg">The argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob{TResult}"/> value that can be used to monitor the status of the background job, as well as waiting for the result
        /// value and the ability to cancel the job.</returns>
        IAsyncJob<TResult> StartNew<TArg, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
            [DisallowNull] Func<TArg, IStatusListener, Task<TResult>> method);

        /// <summary>
        /// Runs a background job asynchronously that returns a value.
        /// </summary>
        /// <typeparam name="TArg">The type of the second argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TResult">The type of the value produced by the background job.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg">The second argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <param name="onComplete">The callback to invoke when the background job is complete.</param>
        /// <returns>The task that is executing the background job and returns the result value.</returns>
        Task<TResult> RunAsync<TArg, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
            [DisallowNull] Func<TArg, IStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null);

        /// <summary>
        /// Starts a new background job that produces a result value.
        /// </summary>
        /// <typeparam name="TResult">The type of the value produced by the background job.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob{TResult}"/> value that can be used to monitor the status of the background job, as well as waiting for the result
        /// value and the ability to cancel the job.</returns>
        IAsyncJob<TResult> StartNew<TResult>([DisallowNull] string title, [DisallowNull] string initialMessage,
            [DisallowNull] Func<IStatusListener, Task<TResult>> method);

        /// <summary>
        /// Runs a background job asynchronously that returns a value.
        /// </summary>
        /// <typeparam name="TResult">The type of the value produced by the background job.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <param name="onComplete">The callback to invoke when the background job is complete.</param>
        /// <returns>The task that is executing the background job and returns the result value.</returns>
        Task<TResult> RunAsync<TResult>([DisallowNull] string title, [DisallowNull] string initialMessage,
            [DisallowNull] Func<IStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null);

        /// <summary>
        /// Starts a new background job.
        /// </summary>
        /// <typeparam name="TArg1">The type of first argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument that is passed to the asynchronous job method.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg1">The first argument that is passed to the asynchronous job method.</param>
        /// <param name="arg2">The second argument that is passed to the asynchronous job method.</param>
        /// <param name="arg3">The third argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob"/> value that can be used to monitor the status of the background job as well as the ability to cancel the job.</returns>
        IAsyncJob StartNew<TArg1, TArg2, TArg3>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
            [DisallowNull] Func<TArg1, TArg2, TArg3, IStatusListener, Task> method);

        /// <summary>
        /// Runs a background job asynchronously.
        /// </summary>
        /// <typeparam name="TArg1">The type of first argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument that is passed to the asynchronous job method.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg1">The first argument that is passed to the asynchronous job method.</param>
        /// <param name="arg2">The second argument that is passed to the asynchronous job method.</param>
        /// <param name="arg3">The third argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <param name="onComplete">The callback to invoke when the background job is complete.</param>
        /// <returns>The task that is executing the background job.</returns>
        Task RunAsync<TArg1, TArg2, TArg3>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
            [DisallowNull] Func<TArg1, TArg2, TArg3, IStatusListener, Task> method, Action<IAsyncJob> onComplete = null);

        /// <summary>
        /// Starts a new background job.
        /// </summary>
        /// <typeparam name="TArg1">The type of first argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument that is passed to the asynchronous job method.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg1">The first argument that is passed to the asynchronous job method.</param>
        /// <param name="arg2">The second argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob"/> value that can be used to monitor the status of the background job as well as the ability to cancel the job.</returns>
        IAsyncJob StartNew<TArg1, TArg2>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
            [DisallowNull] Func<TArg1, TArg2, IStatusListener, Task> method);

        /// <summary>
        /// Runs a background job asynchronously.
        /// </summary>
        /// <typeparam name="TArg1">The type of first argument that is passed to the asynchronous job method.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument that is passed to the asynchronous job method.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg1">The first argument that is passed to the asynchronous job method.</param>
        /// <param name="arg2">The second argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <param name="onComplete">The callback to invoke when the background job is complete.</param>
        /// <returns>The task that is executing the background job.</returns>
        Task RunAsync<TArg1, TArg2>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
            [DisallowNull] Func<TArg1, TArg2, IStatusListener, Task> method, Action<IAsyncJob> onComplete = null);

        /// <summary>
        /// Starts a new background job.
        /// </summary>
        /// <typeparam name="TArg">The type of the argument that is passed to the asynchronous job method.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg">The argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob"/> value that can be used to monitor the status of the background job as well as the ability to cancel the job.</returns>
        IAsyncJob StartNew<TArg>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg, [DisallowNull] Func<TArg, IStatusListener, Task> method);

        /// <summary>
        /// Runs a background job asynchronously.
        /// </summary>
        /// <typeparam name="TArg">The type of the second argument that is passed to the asynchronous job method.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg">The second argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <param name="onComplete">The callback to invoke when the background job is complete.</param>
        /// <returns>The task that is executing the background job.</returns>
        Task RunAsync<TArg>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg, [DisallowNull] Func<TArg, IStatusListener, Task> method,
            Action<IAsyncJob> onComplete = null);

        /// <summary>
        /// Starts a new background job.
        /// </summary>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob"/> value that can be used to monitor the status of the background job as well as the ability to cancel the job.</returns>
        IAsyncJob StartNew([DisallowNull] string title, [DisallowNull] string initialMessage, [DisallowNull] Func<IStatusListener, Task> method);

        /// <summary>
        /// Runs a background job asynchronously.
        /// </summary>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <param name="onComplete">The callback to invoke when the background job is complete.</param>
        /// <returns>The task that is executing the background job.</returns>
        Task RunAsync([DisallowNull] string title, [DisallowNull] string initialMessage, [DisallowNull] Func<IStatusListener, Task> method,
            Action<IAsyncJob> onComplete = null);
    }
}
