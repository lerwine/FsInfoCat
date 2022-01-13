namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a timed asynchronous action that does not return a value.
    /// </summary>
    /// <typeparam name="TEvent">The base type of the push-notification events raised by this asynchronous activity.</typeparam>
    /// <seealso cref="ITimedAsyncActivity" />
    /// <seealso cref="IAsyncAction{TEvent}" />
    public interface ITimedAsyncAction<TEvent> : ITimedAsyncActivity, IAsyncAction<TEvent> where TEvent : ITimedOperationEvent { }

    /// <summary>
    /// Represents a timed asynchronous action that is associated with a user-specified value and does not return a value.
    /// </summary>
    /// <typeparam name="TEvent">The base type of the push-notification events raised by this asynchronous activity.</typeparam>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this asynchronous activity.</typeparam>
    /// <seealso cref="ITimedOperationInfo{TState}" />
    /// <seealso cref="IAsyncAction{TEvent, TState}" />
    /// <seealso cref="ITimedAsyncAction{TEvent}" />
    public interface ITimedAsyncAction<TEvent, TState> : ITimedOperationInfo<TState>, IAsyncAction<TEvent, TState>, ITimedAsyncAction<TEvent> where TEvent : ITimedOperationEvent<TState> { }
}
