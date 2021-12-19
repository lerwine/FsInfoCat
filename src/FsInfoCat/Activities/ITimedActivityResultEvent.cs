namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an event that contains information about the result a timed activity.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="ITimedActivityResultInfo{TResult}" />
    /// <seealso cref="ITimedOperationEvent" />
    /// <seealso cref="IActivityResultEvent{TResult}" />
    public interface ITimedActivityResultEvent<TResult> : ITimedActivityResultInfo<TResult>, ITimedOperationEvent, IActivityResultEvent<TResult> { }

    /// <summary>
    /// Represents an event that contains information about an event for a timed activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="ITimedActivityResultInfo{TState, TResult}" />
    /// <seealso cref="ITimedOperationEvent{TState}" />
    /// <seealso cref="IActivityResultEvent{TState, TResult}" />
    /// <seealso cref="ITimedActivityResultEvent{TResult}" />
    public interface ITimedActivityResultEvent<TState, TResult> : ITimedActivityResultInfo<TState, TResult>, ITimedOperationEvent<TState>, IActivityResultEvent<TState, TResult>, ITimedActivityResultEvent<TResult> { }
}
