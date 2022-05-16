namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes an activity completion event for an activity that produces a result value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <seealso cref="IActivityCompletedEvent" />
    public interface IActivityResultEvent<TResult> : IActivityCompletedEvent
    {
        /// <summary>
        /// Gets the result value.
        /// </summary>
        /// <value>The result that was asynchronously produced or <see langword="default"/> if <see cref="IActivityStatusInfo.StatusValue"/> is
        /// not <see cref="ActivityStatus.RanToCompletion"/>.</value>
        TResult Result { get; }
    }

    /// <summary>
    /// Describes a completion event for an activity that is associated with a user-specified value and produces a result value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <seealso cref="IActivityCompletedEvent{TState}" />
    /// <seealso cref="IActivityResultEvent{TResult}" />
    public interface IActivityResultEvent<TState, TResult> : IActivityCompletedEvent<TState>, IActivityResultEvent<TResult> { }
}
