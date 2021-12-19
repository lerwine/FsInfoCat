namespace FsInfoCat.Activities
{
    /// <summary>
    /// Contains information about the state of a running activity.
    /// </summary>
    /// <seealso cref="IActivityInfo" />
    public interface IOperationInfo : IActivityInfo
    {
        /// <summary>
        /// Gets a value that describes an operation conducted by the activity.
        /// </summary>
        /// <value>The description of the operation conducted by the activity or <see cref="string.Empty"/> if the operation description is not specified.</value>
        string CurrentOperation { get; }
    }

    /// <summary>
    /// Contains information about the state of a running activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="IOperationInfo" />
    public interface IOperationInfo<TState> : IActivityInfo<TState>, IOperationInfo { }
}
