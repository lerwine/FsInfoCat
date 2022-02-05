namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes an activity completion event.
    /// </summary>
    /// <seealso cref="IActivityEvent" />
    /// <seealso cref="IActivityStatusInfo" />
    public interface IActivityCompletedEvent : IActivityEvent, IActivityStatusInfo { }

    /// <summary>
    /// Describes a completion event for an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IActivityEvent{TState}" />
    /// <seealso cref="IActivityStatusInfo{TState}" />
    /// <seealso cref="IActivityCompletedEvent" />
    public interface IActivityCompletedEvent<TState> : IActivityEvent<TState>, IActivityStatusInfo<TState>, IActivityCompletedEvent { }
}
