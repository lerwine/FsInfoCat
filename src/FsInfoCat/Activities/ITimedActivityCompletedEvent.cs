namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes a completion event for a timed activity.
    /// </summary>
    /// <seealso cref="ITimedActivityEvent" />
    /// <seealso cref="IActivityCompletedEvent" />
    public interface ITimedActivityCompletedEvent : ITimedActivityEvent, IActivityCompletedEvent { }

    /// <summary>
    /// Describes a completion event for a timed activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this activity.</typeparam>
    /// <seealso cref="ITimedActivityEvent{TState}" />
    /// <seealso cref="IActivityCompletedEvent{TState}" />
    /// <seealso cref="ITimedActivityCompletedEvent" />
    public interface ITimedActivityCompletedEvent<TState> : ITimedActivityEvent<TState>, IActivityCompletedEvent<TState>, ITimedActivityCompletedEvent { }
}
