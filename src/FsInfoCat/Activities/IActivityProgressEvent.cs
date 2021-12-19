namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes an event that contains progress information about an activity.
    /// </summary>
    /// <seealso cref="IActivityEvent" />
    public interface IActivityProgressEvent : IOperationEvent
    {
        /// <summary>
        /// Gets the completion percentage value.
        /// </summary>
        /// <value>The activity completion percentage value or <see langword="null"/> if not specified.</value>
        byte? PercentComplete { get; }
    }

    /// <summary>
    /// Describes an event that contains progress information about an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="IActivityEvent" />
    public interface IActivityProgressEvent<TState> : IOperationEvent<TState>, IActivityProgressEvent { }
}
