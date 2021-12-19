namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an event that contains information about the result an activity.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="IActivityResultInfo{TResult}" />
    /// <seealso cref="IOperationEvent" />
    public interface IActivityResultEvent<TResult> : IActivityResultInfo<TResult>, IActivityProgressEvent { }

    /// <summary>
    /// Represents an event that contains information about an event for an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="IActivityResultInfo{TState, TResult}" />
    /// <seealso cref="IOperationEvent{TState}" />
    /// <seealso cref="IActivityResultEvent{TResult}" />
    public interface IActivityResultEvent<TState, TResult> : IActivityResultInfo<TState, TResult>, IActivityProgressEvent<TState>, IActivityResultEvent<TResult> { }
}
