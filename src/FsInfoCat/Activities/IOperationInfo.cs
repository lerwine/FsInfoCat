namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents operational information about the state of an activity.
    /// </summary>
    public interface IOperationInfo : IActivityInfo
    {
        /// <summary>
        /// Gets the activity lifecycle status value.
        /// </summary>
        /// <value>An <see cref="ActivityState" /> value that indicates the lifecycle status of the activity.</value>
        ActivityStatus StatusValue { get; }
        /// <summary>
        /// Gets the description of the operation currently being performed.
        /// </summary>
        /// <remarks>This should never be <see langword="null" />.</remarks>
        /// <value>The description of the current operation being performed or <see cref="string.Empty" /> if no operation has been started or no operation description has been provided.</value>
        string CurrentOperation { get; }
        /// <summary>
        /// Gets the percentage completion value.
        /// </summary>
        /// <value>The percentage completion value from <c>0</c> to <c>100</c> or <c>-1</c> if no completion percentage is specified.</value>
        int PercentComplete { get; }
    }

    /// <summary>
    /// Represents operational information about the state of an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this activity.</typeparam>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="IOperationInfo" />
    public interface IOperationInfo<TState> : IActivityInfo<TState>, IOperationInfo { }
}
