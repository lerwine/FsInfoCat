namespace FsInfoCat.Activities
{
    /// <summary>
    /// Contains information about the state of a timed, running activity.
    /// </summary>
    /// <seealso cref="ITimedActivityInfo" />
    /// <seealso cref="IOperationInfo" />
    public interface ITimedOperationInfo : ITimedActivityInfo, IOperationInfo { }

    /// <summary>
    /// Contains information about the state of a timed, running activity that is associated with a user-specified value.
    /// </summary>
    /// <seealso cref="ITimedActivityInfo{TState}" />
    /// <seealso cref="IOperationInfo{TState}" />
    /// <seealso cref="ITimedOperationInfo" />
    public interface ITimedOperationInfo<TState> : ITimedActivityInfo<TState>, IOperationInfo<TState>, ITimedOperationInfo { }
}
