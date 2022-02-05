namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes a timed activity completion event for an activity that produces a result value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <seealso cref="IActivityResultEvent{TResult}" />
    /// <seealso cref="ITimedActivityCompletedEvent" />
    public interface ITimedActivityResultEvent<TResult> : IActivityResultEvent<TResult>, ITimedActivityCompletedEvent { }

    /// <summary>
    /// Describes a completion event for a timed activity that is associated with a user-specified value and produces a result value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <seealso cref="IActivityResultEvent{TState, TResult}" />
    /// <seealso cref="ITimedActivityCompletedEvent{TState}" />
    /// <seealso cref="ITimedActivityResultEvent{TResult}" />
    public interface ITimedActivityResultEvent<TState, TResult> : IActivityResultEvent<TState, TResult>, ITimedActivityCompletedEvent<TState>, ITimedActivityResultEvent<TResult> { }
}
