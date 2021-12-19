using System;
using System.Threading;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Defines a provider for activity progress updates.
    /// </summary>
    /// <typeparam name="TEvent">The type of the progress update event.</typeparam>
    /// <seealso cref="IProgress{TEvent}" />
    /// <seealso cref="IOperationInfo" />
    public interface IAsyncActivityProgress<TEvent> : IProgress<TEvent>, IAsyncActivityProvider, IOperationInfo
        where TEvent : IOperationEvent
    {
        /// <summary>
        /// Gets the cancellation request token.
        /// </summary>
        /// <value>The <see cref="CancellationToken"/> that can be used to check for cancellation requests.</value>
        CancellationToken Token { get; }
    }

    /// <summary>
    /// Defines a provider for progress updates for an activity that is associated with a user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined value associated with the activity.</typeparam>
    /// <typeparam name="TEvent">The type of the progress update event.</typeparam>
    /// <seealso cref="IOperationInfo{TState}" />
    /// <seealso cref="IAsyncActivityProgress{TEvent}" />
    public interface IAsyncActivityProgress<TState, TEvent> : IOperationInfo<TState>, IAsyncActivityProgress<TEvent> where TEvent : IOperationEvent<TState> { }
}
