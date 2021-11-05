using System;

namespace FsInfoCat.Services
{
    public class BgOperationEventArgs : EventArgs, IBgOperationEventArgs
    {
        public Guid ConcurrencyId { get; }

        /// <summary>
        /// Gets the status of the background operation.
        /// </summary>
        public AsyncJobStatus Status { get; }

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about the asynchronous operation.
        /// </summary>
        public object AsyncState { get; }

        /// <summary>
        /// Gets the date and time when the background operation was started.
        /// If <see cref="Status"/> is <see cref="AsyncJobStatus.WaitingToRun"/>, this will be the date and time that the background operation was enqueued.
        /// </summary>
        public DateTime Started { get; }

        /// <summary>
        /// Gets the amount of time that the background operation has been running.
        /// </summary>
        public TimeSpan Elapsed { get; }

        public ActivityCode Activity { get; }

        public MessageCode StatusDescription { get; }

        public string CurrentOperation { get; }

        ActivityCode? IAsyncOperationInfo.Activity => Activity;

        MessageCode? IAsyncOperationInfo.StatusDescription => StatusDescription;

        public BgOperationEventArgs(Guid concurrencyId, DateTime started, TimeSpan elapsed, AsyncJobStatus status, ActivityCode activity, MessageCode statusDescription, string currentOperation = null, object asyncState = null)
            => (ConcurrencyId, Started, Elapsed, Status, Activity, StatusDescription, CurrentOperation, AsyncState) = (concurrencyId, started, elapsed, status, activity, statusDescription, currentOperation.EmptyIfNullOrWhiteSpace(), asyncState);
    }
}
