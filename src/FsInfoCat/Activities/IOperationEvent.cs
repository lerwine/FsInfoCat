namespace FsInfoCat.Activities
{
    /// <summary>
    /// Contains information about an operation event.
    /// </summary>
    /// <seealso cref="IOperationInfo" />
    /// <seealso cref="IActivityEvent" />
    public interface IOperationEvent : IOperationInfo, IActivityEvent { }

    /// <summary>
    /// Contains information about an operation event for an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IOperationInfo{TState}" />
    /// <seealso cref="IActivityEvent{TState}" />
    /// <seealso cref="IOperationEvent" />
    public interface IOperationEvent<TState> : IOperationInfo<TState>, IActivityEvent<TState>, IOperationEvent { }
}
