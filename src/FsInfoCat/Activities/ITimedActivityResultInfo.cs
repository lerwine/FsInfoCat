namespace FsInfoCat.Activities
{
    /// <summary>
    /// Contains information about the result a timed activity.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="ITimedOperationInfo" />
    /// <seealso cref="IActivityResultInfo{TResult}" />
    public interface ITimedActivityResultInfo<TResult> : ITimedOperationInfo, IActivityResultInfo<TResult> { }

    /// <summary>
    /// Contains information about the result a timed activity, whereby the activity is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="ITimedOperationInfo{TState}" />
    /// <seealso cref="IActivityResultInfo{TState, TResult}" />
    /// <seealso cref="ITimedActivityResultInfo{TResult}" />
    public interface ITimedActivityResultInfo<TState, TResult> : ITimedOperationInfo<TState>, IActivityResultInfo<TState, TResult>, ITimedActivityResultInfo<TResult> { }
}
