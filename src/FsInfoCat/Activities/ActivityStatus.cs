namespace FsInfoCat.Activities
{
    /// <summary>
    /// Indicates the lifecycle status of an activity.
    /// </summary>
    public enum ActivityStatus : byte
    {
        /// <summary>
        /// The activity has been scheduled for execution but has not yet begun executing.
        /// </summary>
        WaitingToRun = 2,

        /// <summary>
        /// The activity is running but has not yet completed.
        /// </summary>
        Running = 3,

        /// <summary>
        /// The activity completed execution successfully.
        /// </summary>
        RanToCompletion = 5,

        /// <summary>
        /// The activity was canceled.
        /// </summary>
        Canceled = 6,

        /// <summary>
        /// The activity completed due to an unhandled exception.
        /// </summary>
        Faulted = 7
    }
}
