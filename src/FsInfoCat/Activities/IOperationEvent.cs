namespace FsInfoCat.Activities;

/// <summary>
/// Describes an operational activity event.
/// </summary>
public interface IOperationEvent : IActivityEvent, IOperationInfo { }

/// <summary>
/// Describes an event for an operational activity that is associated with a user-specified value.
/// </summary>
/// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
public interface IOperationEvent<TState> : IActivityEvent<TState>, IOperationInfo<TState>, IOperationEvent { }
