using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an asynchronous action that does not return a value.
    /// </summary>
    /// <typeparam name="TEvent">The base type of the push-notification events raised by this asynchronous activity.</typeparam>
    /// <seealso cref="IObservable{TEvent}" />
    /// <seealso cref="IAsyncActivity" />
    public interface IAsyncAction<TEvent> : IObservable<TEvent>, IAsyncActivity where TEvent : IOperationEvent { }

    /// <summary>
    /// Represents an asynchronous action that is associated with a user-specified value and does not return a value.
    /// </summary>
    /// <typeparam name="TEvent">The base type of the push-notification events raised by this asynchronous activity.</typeparam>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this asynchronous activity.</typeparam>
    /// <seealso cref="IOperationInfo{TState}" />
    /// <seealso cref="IAsyncAction{TEvent}" />
    public interface IAsyncAction<TEvent, TState> : IOperationInfo<TState>, IAsyncAction<TEvent> where TEvent : IOperationEvent<TState> { }
}
