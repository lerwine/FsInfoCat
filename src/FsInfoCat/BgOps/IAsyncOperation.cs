namespace FsInfoCat.BgOps
{
    /// <summary>
    /// Represents an asynchronous operation.
    /// </summary>
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IAsyncOperation : ICustomAsyncOperation<IAsyncOpEventArgs>
    //public interface IAsyncOperation : IAsyncOpStatus, IObservable<IAsyncOpEventArgs>
    {
        ///// <summary>
        ///// Gets the <see cref="Task"/> for the asynchronous operation.
        ///// </summary>
        //Task Task { get; }

        ///// <summary>
        /////  Indicates whether cancellation has been requested for this token.
        ///// </summary>
        //bool IsCancellationRequested { get; }

        //IAsyncOpEventArgs LastEvent { get; }

        ///// <summary>
        ///// Cancels the background operation;
        ///// </summary>
        //void Cancel();

        ///// <summary>
        ///// Cancels the background operation after a specified number of milliseconds.
        ///// </summary>
        ///// <param name="millisecondsDelay">Number of miilliseconds to wait before cancelling the background operation.</param>
        //void CancelAfter(int millisecondsDelay);

        ///// <summary>
        ///// Cancels the background operation after a specified duration.
        ///// </summary>
        ///// <param name="delay">Duration of delay before cancelling the background operation.</param>
        //void CancelAfter(TimeSpan delay);

        ///// <summary>
        ///// Gets the parent asynchronous operation or <see langword="null"/> if there is no parent operation.
        ///// </summary>
        ///// <remarks>This serves the same conceptual puropose as the
        ///// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ParentActivityId">ProgressRecord.ParentActivityId</see> property.</remarks>
        //new IAsyncOpEventArgs ParentOperation { get; }
    }

    /// <summary>
    /// Represents an asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IAsyncOperation<TState> : IAsyncOperation, ICustomAsyncOperation<TState, IAsyncOpEventArgs<TState>>
    //public interface IAsyncOperation<TState> : IAsyncOperation, IAsyncOpStatus<TState>, IObservable<IAsyncOpEventArgs<TState>>
    {
        //new IAsyncOpEventArgs<TState> LastEvent { get; }
    }
}
