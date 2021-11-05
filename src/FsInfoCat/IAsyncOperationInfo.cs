using System;

namespace FsInfoCat
{
    public interface IAsyncOperationInfo
    {
        Guid ConcurrencyId { get; }

        /// <summary>
        /// Gets the status of the background operation.
        /// </summary>
        AsyncJobStatus Status { get; }

        ActivityCode? Activity { get; }

        MessageCode? StatusDescription { get; }

        string CurrentOperation { get; }

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about the asynchronous operation.
        /// </summary>
        object AsyncState { get; }
    }
}
