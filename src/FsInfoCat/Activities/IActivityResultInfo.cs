namespace FsInfoCat.Activities
{
    /// <summary>
    /// Contains information about the result an activity.
    /// </summary>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="IActivityInfo" />
    public interface IActivityResultInfo<TResult> : IActivityInfo
    {
        /// <summary>
        /// Gets the result produced by an activity.
        /// </summary>
        /// <value>The <typeparamref name="TResult"/> value produced by the activity.</value>
        TResult Result { get; }
    }

    /// <summary>
    /// Contains information about an event for an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <typeparam name="TResult">The type of result value produced by the activity.</typeparam>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="IActivityResultInfo{TResult}" />
    public interface IActivityResultInfo<TState, TResult> : IActivityInfo<TState>, IActivityResultInfo<TResult> { }
}
