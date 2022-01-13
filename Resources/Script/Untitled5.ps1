#Info
<#
    /// <summary>
    /// Contains information about the state of an activity.
    /// </summary>
    public interface IActivityInfo
    {
        /// <summary>
        /// Gets the unique identifier of the described activity.
        /// </summary>
        /// <value>The <see cref="Guid"/> value that is unique to the described activity.</value>
        Guid ActivityId { get; }

        /// <summary>
        /// Gets the unique identifier of the parent activity.
        /// </summary>
        /// <value>The <see cref="Guid"/> value that is unique to the parent activity or <see langword="null"/> if there is no parent activity.</value>
        Guid? ParentActivityId { get; }

        /// <summary>
        /// Gets the short description of the activity.
        /// </summary>
        /// <value>A <see cref="string"/> that describes the activity.</value>
        /// <remarks>This should never be <see langword="null"/> or <see cref="string.Empty"/>.</remarks>
        string ShortDescription { get; }

        /// <summary>
        /// Gets the description of the activity's status.
        /// </summary>
        /// <value>A <see cref="string"/> that gives a verbose description the status for the activity.</value>
        /// <remarks>This should never be <see langword="null"/> or <see cref="string.Empty"/>.</remarks>
        string StatusDescription { get; }
    }

    /// <summary>
    /// Contains information about the state of an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IActivityInfo" />
    public interface IActivityInfo<TState> : IActivityInfo
    {
        /// <summary>
        /// Gets the user-specified value that is associated with the activity.
        /// </summary>
        /// <value>The <typeparamref name="TState"/> value that is associated with the described activity.</value>
        TState AsyncState { get; }
    }
    /// <summary>
    /// Contains information about the state of a running activity.
    /// </summary>
    /// <seealso cref="IActivityInfo" />
    public interface IOperationInfo : IActivityInfo
    {
        /// <summary>
        /// Gets the lifecycle status value.
        /// </summary>
        /// <value>An <see cref="ActivityState"/> value that indicates the lifecycle status of the activity.</value>
        ActivityStatus StatusValue { get; }

        /// <summary>
        /// Gets the description of the operation currently being performed.
        /// </summary>
        /// <value>The description of the current operation being performed or <see cref="string.Empty"/> if no operation has been started or no operation description has been provided.</value>
        /// <remarks>This should never be <see langword="null"/>.</remarks>
        string CurrentOperation { get; }

        /// <summary>
        /// Gets the percentage completion value.
        /// </summary>
        /// <value>The percentage completion value or <see langword="null"/> if no completion percentage is specified.</value>
        int? PercentComplete { get; }
    }

    /// <summary>
    /// Contains information about the state of a running activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="IOperationInfo" />
    public interface IOperationInfo<TState> : IActivityInfo<TState>, IOperationInfo { }
#>
#Event
<#
    /// <summary>
    /// Contains information about an activity event.
    /// </summary>
    /// <seealso cref="IActivityInfo" />
    public interface IActivityEvent : IActivityInfo
    {
        /// <summary>
        /// Gets the exception (if any) associated with the event.
        /// </summary>
        /// <value>The <see cref="Exception"/> associated with the event or <see langword="null"/> if there is none.</value>
        Exception Exception { get; }
    }

    /// <summary>
    /// Interface IActivityEvent
    /// </summary>
    /// <typeparam name="TState">The type of the t state.</typeparam>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="IActivityEvent" />
    public interface IActivityEvent<TState> : IActivityInfo<TState>, IActivityEvent { }
    /// <summary>
    /// Contains information about an operation event.
    /// </summary>
    /// <seealso cref="IOperationInfo" />
    /// <seealso cref="IActivityEvent" />
    public interface IOperationEvent : IOperationInfo, IActivityEvent { }

    /// <summary>
    /// Contains information about an operation event for an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IOperationInfo{TState}" />
    /// <seealso cref="IActivityEvent{TState}" />
    /// <seealso cref="IOperationEvent" />
    public interface IOperationEvent<TState> : IOperationInfo<TState>, IActivityEvent<TState>, IOperationEvent { }
#>
#AsyncActivity
<#
    /// <summary>
    /// Represents an asynchronous activity.
    /// </summary>
    /// <seealso cref="IOperationInfo" />
    public interface IAsyncActivity : IOperationInfo
    {
        /// <summary>
        /// Gets the task for the asyncronous activity.
        /// </summary>
        /// <value>The task for the asyncronous activity.</value>
        Task Task { get; }

        /// <summary>
        /// Gets the token source that can be used to cancel the asyncronous activity.
        /// </summary>
        /// <value>The token source that can be used to cancel the asyncronous activity.</value>
        CancellationTokenSource TokenSource { get; }
    }
    /// <summary>
    /// Represents an asynchronous action that does not return a value.
    /// </summary>
    /// <typeparam name="TEvent">The type of the progress notification event.</typeparam>
    /// <seealso cref="IOperationEvent" />
    /// <seealso cref="IObservable{TEvent}" />
    public interface IAsyncAction<TEvent> : IAsyncActivity, IObservable<TEvent> where TEvent : IOperationEvent
    {
    }

    /// <summary>
    /// Represents an asynchronous action that is associated with a user-specified value anddoes not return a value.
    /// </summary>
    /// <typeparam name="TState">The type of the user defined value to be associated with the asynchonous action.</typeparam>
    /// <typeparam name="TEvent">The type of the progress notification event.</typeparam>
    /// <seealso cref="IOperationEvent{TState}" />
    /// <seealso cref="IObservable{TEvent}" />
    /// <seealso cref="IAsyncAction{TEvent}" />
    public interface IAsyncAction<TState, TEvent> : IOperationInfo<TState>, IAsyncAction<TEvent> where TEvent : IOperationEvent<TState> { }
    /// <summary>
    /// Represents an asynchronous activity that produces a result value.
    /// </summary>
    /// <typeparam name="TEvent">The type of the progress notification event.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <seealso cref="IAsyncAction{TEvent}" />
    /// <seealso cref="IObservable{TEvent}" />
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
    /// <typeparam name="TState">The type of the user defined value to be associated with the asynchonous function.</typeparam>
    /// <typeparam name="TEvent">The type of the progress notification event.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <seealso cref="IAsyncAction{TState, TEvent}" />
    /// <seealso cref="IAsyncFunc{TEvent, TResult}" />
    public interface IAsyncFunc<TState, TEvent, TResult> : IAsyncAction<TState, TEvent> where TEvent : IOperationEvent<TState> { }
#>
#Provider
<#
    public interface IAsyncActivitySource : IReadOnlyCollection<IAsyncActivity>
    {
        IDisposable SubscribeStateChange(IObserver<IAsyncActivity> observer, Action<IAsyncActivity[]> onObserving);

        IObservable<IAsyncActivity> StateChangeObservable { get; }
    }
    public interface IAsyncActivityProvider : IAsyncActivitySource
    {
        IAsyncAction<IActivityEvent> InvokeAsync(string activityDescription, string initialStatusDescription, Func<IActivityProgress, Task> asyncMethodDelegate);

        IAsyncFunc<IActivityEvent, TResult> InvokeAsync<TResult>(string activityDescription, string initialStatusDescription, Func<IActivityProgress, Task<TResult>> asyncMethodDelegate);

        ITimedAsyncAction<ITimedActivityEvent> InvokeTimedAsync(string activityDescription, string initialStatusDescription, Func<IActivityProgress, Task> asyncMethodDelegate);

        ITimedAsyncFunc<ITimedActivityEvent, TResult> InvokeTimedAsync<TResult>(string activityDescription, string initialStatusDescription, Func<IActivityProgress, Task<TResult>> asyncMethodDelegate);

        IAsyncAction<TState, IActivityEvent<TState>> InvokeAsync<TState>(string activityDescription, string initialStatusDescription, Func<IActivityProgress<TState>, Task> asyncMethodDelegate);

        IAsyncFunc<TState, IActivityEvent<TState>, TResult> InvokeAsync<TState, TResult>(string activityDescription, string initialStatusDescription, Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate);

        ITimedAsyncAction<TState, ITimedActivityEvent<TState>> InvokeTimedAsync<TState>(string activityDescription, string initialStatusDescription, Func<IActivityProgress<TState>, Task> asyncMethodDelegate);

        ITimedAsyncFunc<TState, ITimedActivityEvent<TState>, TResult> InvokeTimedAsync<TState, TResult>(string activityDescription, string initialStatusDescription, Func<IActivityProgress<TState>, Task<TResult>> asyncMethodDelegate);
    }
    public interface IAsyncActivityService : IHostedService, IAsyncActivityProvider, IObservable<bool>
    {
        bool IsActive { get; }
    }
    public interface IActivityProgress : IOperationInfo, IAsyncActivityProvider, IProgress<string>, IProgress<Exception>, IProgress<int>
    {
        void Report(Exception error, string statusDescription, string currentOperation, int percentComplete);

        void Report(string statusDescription, string currentOperation, int percentComplete);

        void Report(Exception error, string statusDescription, string currentOperation);

        void Report(string statusDescription, string currentOperation);

        void Report(Exception error, string statusDescription);

        void ClearPercentComplete(Exception error, string statusDescription, string currentOperation);

        void ClearPercentComplete(string statusDescription, string currentOperation);

        void ClearPercentComplete(Exception error, string statusDescription);

        void ClearPercentComplete(Exception error);

        void ClearPercentComplete(string statusDescription);

        void ReportCurrentOperation(Exception error, string currentOperation, int percentComplete);

        void ReportCurrentOperation(string currentOperation, int percentComplete);

        void ReportCurrentOperation(Exception error, string currentOperation);

        void ReportCurrentOperation(string currentOperation);
    }

    public interface IActivityProgress<TState> : IOperationEvent<TState>, IActivityProgress { }
#>