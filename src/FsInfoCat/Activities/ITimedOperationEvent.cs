namespace FsInfoCat.Activities;

/// <summary>
/// Describes an event for a timed operational activity.
/// </summary>
public interface ITimedOperationEvent : IOperationEvent, ITimedOperationInfo, ITimedActivityEvent { }

/// <summary>
/// Describes an event for a timed operational activity that is associated with a user-specified value.
/// </summary>
/// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
public interface ITimedOperationEvent<TState> : IOperationEvent<TState>, ITimedOperationInfo<TState>, ITimedActivityEvent<TState>, ITimedOperationEvent { }
