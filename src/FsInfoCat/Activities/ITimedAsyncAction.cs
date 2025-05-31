namespace FsInfoCat.Activities;

/// <summary>
/// Represents a timed asynchronous action that does not return a value.
/// </summary>
/// <typeparam name="TEvent">The base type of the push-notification events raised by this asynchronous activity.</typeparam>
public interface ITimedAsyncAction<TEvent> : ITimedAsyncActivity, IAsyncAction<TEvent> where TEvent : ITimedActivityEvent { }

/// <summary>
/// Represents a timed asynchronous action that is associated with a user-specified value and does not return a value.
/// </summary>
/// <typeparam name="TEvent">The base type of the push-notification events raised by this asynchronous activity.</typeparam>
/// <typeparam name="TState">The type of the user-defined value that is associated with this asynchronous activity.</typeparam>
public interface ITimedAsyncAction<TEvent, TState> : ITimedOperationInfo<TState>, IAsyncAction<TEvent, TState>, ITimedAsyncAction<TEvent> where TEvent : ITimedActivityEvent<TState> { }
