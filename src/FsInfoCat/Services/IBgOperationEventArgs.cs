using System;

namespace FsInfoCat.Services
{
    public interface IBgOperationEventArgs : IAsyncOperationInfo
    {
        /// <summary>
        /// Gets the date and time when the background operation was started.
        /// If <see cref="Status"/> is <see cref="AsyncJobStatus.WaitingToRun"/>, this will be the date and time that the background operation was enqueued.
        /// </summary>
        DateTime Started { get; }

        /// <summary>
        /// Gets the amount of time that the background operation has been running.
        /// </summary>
        TimeSpan Elapsed { get; }

        new ActivityCode Activity { get; }

        new MessageCode StatusDescription { get; }
    }
}
