namespace FsInfoCat.Activities
{
    public interface IActivityStatusInfo : IActivityInfo
    {
        /// <summary>
        /// Gets the activity lifecycle status value.
        /// </summary>
        /// <value>An <see cref="ActivityState" /> value that indicates the lifecycle status of the activity.</value>
        ActivityStatus StatusValue { get; }
    }

    public interface IActivityStatusInfo<TState> : IActivityInfo<TState>, IActivityStatusInfo { }
}
