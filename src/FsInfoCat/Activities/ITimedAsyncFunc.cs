namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a timed asynchronous activity that produces a result value.
    /// </summary>
    /// <typeparam name="TEvent">The base type of the push-notification events raised by this activity.</typeparam>
    /// <typeparam name="TResult">The type of the result value produced by this asynchronous function.</typeparam>
    /// <seealso cref="IAsyncFunc{TEvent, TResult}" />
    /// <seealso cref="ITimedAsyncAction{TEvent}" />
    public interface ITimedAsyncFunc<TEvent, TResult> : IAsyncFunc<TEvent, TResult>, ITimedAsyncAction<TEvent> where TEvent : ITimedActivityEvent { }

    /// <summary>
    /// Represents a timed asynchronous activity that is associated with a user-specified value and produces a result value.
    /// </summary>
    /// <typeparam name="TEvent">The base type of the push-notification events raised by this activity.</typeparam>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this asynchronous function.</typeparam>
    /// <typeparam name="TResult">The type of the result value produced by this asynchronous function.</typeparam>
    /// <seealso cref="IAsyncFunc{TEvent, TState, TResult}" />
    /// <seealso cref="ITimedAsyncAction{TEvent, TState}" />
    /// <seealso cref="ITimedAsyncFunc{TEvent, TResult}" />
    public interface ITimedAsyncFunc<TEvent, TState, TResult> : IAsyncFunc<TEvent, TState, TResult>, ITimedAsyncAction<TEvent, TState>, ITimedAsyncFunc<TEvent, TResult> where TEvent : ITimedActivityEvent<TState> { }
}
