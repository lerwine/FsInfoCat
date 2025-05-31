namespace FsInfoCat.Activities;

/// <summary>
/// Describes an operational activity termination event.
/// </summary>
public record OperationTerminatedEvent : OperationEvent, IActivityCompletedEvent { }

/// <summary>
/// Describes an operational activity termination event for an activity that is associated with a user-specified value.
/// </summary>
/// <typeparam name="TState">The type of the user specified value associated with the described operational activity.</typeparam>
public record OperationTerminatedEvent<TState> : OperationEvent<TState>, IActivityCompletedEvent<TState> { }
