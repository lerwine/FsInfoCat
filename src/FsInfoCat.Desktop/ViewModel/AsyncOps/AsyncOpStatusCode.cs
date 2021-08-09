namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Indicates the status of a background operation.
    /// </summary>
    public enum AsyncOpStatusCode
    {
        /// <summary>
        /// The background operation has not yet been started.
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// The background operation is currently in progress.
        /// </summary>
        Running = 1,

        /// <summary>
        /// The background operation was notified of a cancellation, but has not yet completed.
        /// </summary>
        CancellationPending = 2,

        /// <summary>
        /// The background operation ran to completion without cancellation or any unhandled exception.
        /// </summary>
        RanToCompletion = 3,

        /// <summary>
        /// The background operation completed due to an unhandled exception.
        /// </summary>
        Faulted = 4,

        /// <summary>
        /// The background operation has been canceled.
        /// </summary>
        Canceled = 5
    }
}
