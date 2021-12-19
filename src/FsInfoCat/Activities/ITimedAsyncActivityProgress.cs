using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Defines a provider for timed activity progress updates.
    /// </summary>
    /// <typeparam name="TEvent">The type of the progress update event.</typeparam>
    /// <seealso cref="ITimedOperationInfo" />
    /// <seealso cref="IAsyncActivityProgress{TEvent}" />
    public interface ITimedAsyncActivityProgress<TEvent> : ITimedOperationInfo, IAsyncActivityProgress<TEvent> where TEvent : ITimedOperationEvent { }

    /// <summary>
    /// Defines a provider for progress updates for a timed activity that is associated with a user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined value associated with the activity.</typeparam>
    /// <typeparam name="TEvent">The type of the progress update event.</typeparam>
    /// <seealso cref="ITimedOperationInfo{TState}" />
    /// <seealso cref="IAsyncActivityProgress{TState, TEvent}" />
    /// <seealso cref="ITimedAsyncActivityProgress{TEvent}" />
    public interface ITimedAsyncActivityProgress<TState, TEvent> : ITimedOperationInfo<TState>, IAsyncActivityProgress<TState, TEvent>, ITimedAsyncActivityProgress<TEvent> where TEvent : ITimedOperationEvent<TState> { }
}
