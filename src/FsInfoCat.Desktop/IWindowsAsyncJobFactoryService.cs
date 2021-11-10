using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// A service that can be used to create background jobs that can indicate progress/status information.
    /// </summary>
    /// <seealso cref="IAsyncJobFactoryService" />
    public interface IWindowsAsyncJobFactoryService : IBgOperationQueue, IAsyncJobFactoryService
    {
        /// <summary>
        /// Sets the view model to use for displaying background task status while the window is open.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="viewModel">The view model that will be used to present active background tasks.</param>
        void SetWindowViewModel([DisallowNull] System.Windows.Window window, [DisallowNull] ViewModel.AsyncOps.JobFactoryServiceViewModel viewModel);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg1, TArg2, TArg3, Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task<TResult>>) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg1, TArg2, TArg3, Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task<TResult>>)")]
        IAsyncJob<TResult> StartNew<TArg1, TArg2, TArg3, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
                [DisallowNull] Func<TArg1, TArg2, TArg3, IWindowsStatusListener, Task<TResult>> method);

        IQueuedBgProducer<TResult> Enqueue<TArg1, TArg2, TArg3, TResult>(ActivityCode activity, MessageCode statusMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3, [DisallowNull] Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task<TResult>> asyncMethod);

        IQueuedBgProducer<TResult> Enqueue<TArg1, TArg2, TArg3, TResult>(ActivityCode activity, TArg1 arg1, TArg2 arg2, TArg3 arg3, [DisallowNull] Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task<TResult>> asyncMethod);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg1, TArg2, TArg3, Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task<TResult>>) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg1, TArg2, TArg3, Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task<TResult>>)")]
        Task<TResult> RunAsync<TArg1, TArg2, TArg3, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
                [DisallowNull] Func<TArg1, TArg2, TArg3, IWindowsStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg1, TArg2, Func<IWindowsOperationProgress, TArg1, TArg2, Task<TResult>>) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg1, TArg2, Func<IWindowsOperationProgress, TArg1, TArg2, Task<TResult>>)")]
        IAsyncJob<TResult> StartNew<TArg1, TArg2, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
                [DisallowNull] Func<TArg1, TArg2, IWindowsStatusListener, Task<TResult>> method);

        IQueuedBgProducer<TResult> Enqueue<TArg1, TArg2, TResult>(ActivityCode activity, MessageCode statusMessage, TArg1 arg1, TArg2 arg2, [DisallowNull] Func<IWindowsOperationProgress, TArg1, TArg2, Task<TResult>> asyncMethod);

        IQueuedBgProducer<TResult> Enqueue<TArg1, TArg2, TResult>(ActivityCode activity, TArg1 arg1, TArg2 arg2, [DisallowNull] Func<IWindowsOperationProgress, TArg1, TArg2, Task<TResult>> asyncMethod);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg1, TArg2, Func<IWindowsOperationProgress, TArg1, TArg2, Task<TResult>>) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg1, TArg2, Func<IWindowsOperationProgress, TArg1, TArg2, Task<TResult>>)")]
        Task<TResult> RunAsync<TArg1, TArg2, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
                [DisallowNull] Func<TArg1, TArg2, IWindowsStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg, Func<IWindowsOperationProgress, TArg, Task<TResult>>) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg, Func<IWindowsOperationProgress, TArg, Task<TResult>>)")]
        IAsyncJob<TResult> StartNew<TArg, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
            [DisallowNull] Func<TArg, IWindowsStatusListener, Task<TResult>> method);

        IQueuedBgProducer<TResult> Enqueue<TArg, TResult>(ActivityCode activity, MessageCode statusMessage, TArg arg, [DisallowNull] Func<IWindowsOperationProgress, TArg, Task<TResult>> asyncMethod);

        IQueuedBgProducer<TResult> Enqueue<TArg, TResult>(ActivityCode activity, TArg arg, [DisallowNull] Func<IWindowsOperationProgress, TArg, Task<TResult>> asyncMethod);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg, Func<IWindowsOperationProgress, TArg, Task<TResult>>) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg, Func<IWindowsOperationProgress, TArg, Task<TResult>>)")]
        Task<TResult> RunAsync<TArg, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
            [DisallowNull] Func<TArg, IWindowsStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null);

        /// <summary>
        /// Starts a new background job that produces a result value.
        /// </summary>
        /// <typeparam name="TResult">The type of the value produced by the background job.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob{TResult}"/> value that can be used to monitor the status of the background job, as well as waiting for the result
        /// value and the ability to cancel the job.</returns>
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, Func<IWindowsOperationProgress, Task<TResult>>) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, Func<IWindowsOperationProgress, Task<TResult>>)")]
        IAsyncJob<TResult> StartNew<TResult>([DisallowNull] string title, [DisallowNull] string initialMessage,
            [DisallowNull] Func<IWindowsStatusListener, Task<TResult>> method);

        IQueuedBgProducer<TResult> Enqueue<TResult>(ActivityCode activity, MessageCode statusMessage, [DisallowNull] Func<IWindowsOperationProgress, Task<TResult>> asyncMethod);

        IQueuedBgProducer<TResult> Enqueue<TResult>(ActivityCode activity, [DisallowNull] Func<IWindowsOperationProgress, Task<TResult>> asyncMethod);

        /// <summary>
        /// Runs a background job asynchronously that returns a value.
        /// </summary>
        /// <typeparam name="TResult">The type of the value produced by the background job.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <param name="onComplete">The callback to invoke when the background job is complete.</param>
        /// <returns>The task that is executing the background job and returns the result value.</returns>
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, Func<IWindowsOperationProgress, Task<TResult>>) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, Func<IWindowsOperationProgress, Task<TResult>>)")]
        Task<TResult> RunAsync<TResult>([DisallowNull] string title, [DisallowNull] string initialMessage,
            [DisallowNull] Func<IWindowsStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg1, TArg2, TArg3, Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg1, TArg2, TArg3, Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task)")]
        IAsyncJob StartNew<TArg1, TArg2, TArg3>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
                [DisallowNull] Func<TArg1, TArg2, TArg3, IWindowsStatusListener, Task> method);

        IQueuedBgOperation Enqueue<TArg1, TArg2, TArg3>(ActivityCode activity, MessageCode statusMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3, [DisallowNull] Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task> asyncMethod);

        IQueuedBgOperation Enqueue<TArg1, TArg2, TArg3>(ActivityCode activity, TArg1 arg1, TArg2 arg2, TArg3 arg3, [DisallowNull] Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task> asyncMethod);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg1, TArg2, TArg3, Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg1, TArg2, TArg3, Func<IWindowsOperationProgress, TArg1, TArg2, TArg3, Task)")]
        Task RunAsync<TArg1, TArg2, TArg3>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
                [DisallowNull] Func<TArg1, TArg2, TArg3, IWindowsStatusListener, Task> method, Action<IAsyncJob> onComplete = null);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg1, TArg2, Func<IWindowsOperationProgress, TArg1, TArg2, Task) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg1, TArg2, Func<IWindowsOperationProgress, TArg1, TArg2, Task)")]
        IAsyncJob StartNew<TArg1, TArg2>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
            [DisallowNull] Func<TArg1, TArg2, IWindowsStatusListener, Task> method);

        IQueuedBgOperation Enqueue<TArg1, TArg2>(ActivityCode activity, MessageCode statusMessage, TArg1 arg1, TArg2 arg2, [DisallowNull] Func<IWindowsOperationProgress, TArg1, TArg2, Task> asyncMethod);

        IQueuedBgOperation Enqueue<TArg1, TArg2>(ActivityCode activity, TArg1 arg1, TArg2 arg2, [DisallowNull] Func<IWindowsOperationProgress, TArg1, TArg2, Task> asyncMethod);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg1, TArg2, Func<IWindowsOperationProgress, TArg1, TArg2, Task) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg1, TArg2, Func<IWindowsOperationProgress, TArg1, TArg2, Task)")]
        Task RunAsync<TArg1, TArg2>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
            [DisallowNull] Func<TArg1, TArg2, IWindowsStatusListener, Task> method, Action<IAsyncJob> onComplete = null);

        /// <summary>
        /// Starts a new background job.
        /// </summary>
        /// <typeparam name="TArg">The type of the argument that is passed to the asynchronous job method.</typeparam>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="arg">The argument that is passed to the asynchronous job method.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob"/> value that can be used to monitor the status of the background job as well as the ability to cancel the job.</returns>
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg, Func<IWindowsOperationProgress, TArg, Task) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg, Func<IWindowsOperationProgress, TArg, Task)")]
        IAsyncJob StartNew<TArg>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
            [DisallowNull] Func<TArg, IWindowsStatusListener, Task> method);

        IQueuedBgOperation Enqueue<TArg>(ActivityCode activity, MessageCode statusMessage, TArg arg, [DisallowNull] Func<IWindowsOperationProgress, TArg, Task> asyncMethod);

        IQueuedBgOperation Enqueue<TArg>(ActivityCode activity, TArg arg, [DisallowNull] Func<IWindowsOperationProgress, TArg, Task> asyncMethod);

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
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, TArg, Func<IWindowsOperationProgress, TArg, Task) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, TArg, Func<IWindowsOperationProgress, TArg, Task)")]
        Task RunAsync<TArg>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
            [DisallowNull] Func<TArg, IWindowsStatusListener, Task> method, Action<IAsyncJob> onComplete = null);

        /// <summary>
        /// Starts a new background job.
        /// </summary>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <returns>An <see cref="IAsyncJob"/> value that can be used to monitor the status of the background job as well as the ability to cancel the job.</returns>
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, Func<IWindowsOperationProgress, TArg1, TArg2, Task) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, Func<IWindowsOperationProgress, Task)")]
        IAsyncJob StartNew([DisallowNull] string title, [DisallowNull] string initialMessage, [DisallowNull] Func<IWindowsStatusListener, Task> method);

        IQueuedBgOperation Enqueue<TArg>(ActivityCode activity, MessageCode statusMessage, [DisallowNull] Func<IWindowsOperationProgress, TArg, Task> asyncMethod);

        IQueuedBgOperation Enqueue<TArg>(ActivityCode activity, [DisallowNull] Func<IWindowsOperationProgress, TArg, Task> asyncMethod);

        /// <summary>
        /// Runs a background job asynchronously.
        /// </summary>
        /// <param name="title">The title of the background job.</param>
        /// <param name="initialMessage">The initial status message to display for the background job.</param>
        /// <param name="method">The asynchronous method to execute as the background job.</param>
        /// <param name="onComplete">The callback to invoke when the background job is complete.</param>
        /// <returns>The task that is executing the background job.</returns>
        [Obsolete("Use IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, MessageCode, Func<IWindowsOperationProgress, TArg1, TArg2, Task) or IWindowsAsyncJobFactoryService.Enqueue(ActivityCode activity, Func<IWindowsOperationProgress, Task)")]
        Task RunAsync([DisallowNull] string title, [DisallowNull] string initialMessage, [DisallowNull] Func<IWindowsStatusListener, Task> method,
            Action<IAsyncJob> onComplete = null);
    }
}
