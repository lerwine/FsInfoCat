namespace FsInfoCat.Activities
{
    /// <summary>
    /// Contains information about an timed activity event.
    /// </summary>
    /// <seealso cref="ITimedActivityInfo" />
    /// <seealso cref="IActivityEvent" />
    public interface ITimedActivityEvent : ITimedActivityInfo, IActivityEvent { }

    /// <summary>
    /// Contains information about an event for a timed activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="ITimedActivityInfo{TState}" />
    /// <seealso cref="IActivityEvent{TState}" />
    /// <seealso cref="ITimedActivityEvent" />
    public interface ITimedActivityEvent<TState> : ITimedActivityInfo<TState>, IActivityEvent<TState>, ITimedActivityEvent { }
}
