namespace FsInfoCat.Activities
{
    /// <summary>
    /// Contains information about an operation event for a timed activity.
    /// </summary>
    /// <seealso cref="ITimedOperationInfo" />
    /// <seealso cref="ITimedActivityEvent" />
    /// <seealso cref="IOperationEvent" />
    public interface ITimedOperationEvent : ITimedOperationInfo, ITimedActivityEvent, IOperationEvent { }

    /// <summary>
    /// Contains information about an operation event for a timed activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="ITimedOperationInfo{TState}" />
    /// <seealso cref="ITimedActivityEvent{TState}" />
    /// <seealso cref="IOperationEvent{TState}" />
    /// <seealso cref="ITimedOperationEvent" />
    public interface ITimedOperationEvent<TState> : ITimedOperationInfo<TState>, ITimedActivityEvent<TState>, IOperationEvent<TState>, ITimedOperationEvent { }
}
