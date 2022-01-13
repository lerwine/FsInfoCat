namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes an operational activity event.
    /// </summary>
    /// <seealso cref="IActivityEvent" />
    /// <seealso cref="IOperationInfo" />
    public interface IOperationEvent : IActivityEvent, IOperationInfo { }

    /// <summary>
    /// Describes an event for an operational activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IActivityEvent{TState}" />
    /// <seealso cref="IOperationInfo{TState}" />
    /// <seealso cref="IOperationEvent" />
    public interface IOperationEvent<TState> : IActivityEvent<TState>, IOperationInfo<TState>, IOperationEvent { }
}
