namespace FsInfoCat.Activities
{
    /// <summary>
    /// Describes an event that contains progress information about a timed activity.
    /// </summary>
    /// <seealso cref="ITimedOperationEvent" />
    /// <seealso cref="IActivityProgressEvent" />
    public interface ITimedActivityProgressEvent : ITimedOperationEvent, IActivityProgressEvent { }

    /// <summary>
    /// Describes an event that contains progress information about a timed activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="ITimedOperationEvent{TState}" />
    /// <seealso cref="IActivityProgressEvent{TState}" />
    /// <seealso cref="ITimedActivityProgressEvent" />
    public interface ITimedActivityProgressEvent<TState> : ITimedOperationEvent<TState>, IActivityProgressEvent<TState>, ITimedActivityProgressEvent { }
}
