namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes an event for a timed activity.
    /// </summary>
    /// <seealso cref="ITimedActivityInfo" />
    /// <seealso cref="IActivityEvent" />
    public interface ITimedActivityEvent : ITimedActivityInfo, IActivityEvent { }

    /// <summary>
    /// Describes an event for a timed activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this activity.</typeparam>
    /// <seealso cref="ITimedActivityInfo{TState}" />
    /// <seealso cref="IActivityEvent{TState}" />
    /// <seealso cref="ITimedActivityEvent" />
    public interface ITimedActivityEvent<TState> : ITimedActivityInfo<TState>, IActivityEvent<TState>, ITimedActivityEvent { }
}
