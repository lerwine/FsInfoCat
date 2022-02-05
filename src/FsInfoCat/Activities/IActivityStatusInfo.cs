namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents information about the lifecycle status of an activity.
    /// </summary>
    public interface IActivityStatusInfo : IActivityInfo
    {
        /// <summary>
        /// Gets the activity lifecycle status value.
        /// </summary>
        /// <value>An <see cref="ActivityState" /> value that indicates the lifecycle status of the activity.</value>
        ActivityStatus StatusValue { get; }
    }

    /// <summary>
    /// Represents information about the lifecycle status of an activity that is associated with a user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this activity.</typeparam>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="IActivityStatusInfo" />
    public interface IActivityStatusInfo<TState> : IActivityInfo<TState>, IActivityStatusInfo { }
}
