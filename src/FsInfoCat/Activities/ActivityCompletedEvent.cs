namespace FsInfoCat.Activities;

/// <summary>
/// Describes an activity completion event.
/// </summary>
public record ActivityCompletedEvent : ActivityEvent, IActivityCompletedEvent
{
    /// <summary>
    /// Gets the value that indicates the lifecycle status of the activity.
    /// </summary>
    public ActivityStatus StatusValue { get; init; }
}

/// <summary>
/// Describes a completion event for an activity that is associated with a user-specified value.
/// </summary>
/// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
public record ActivityCompletedEvent<TState> : ActivityCompletedEvent, IActivityCompletedEvent<TState>
{
    /// <summary>
    /// Gets the user-defined value that is associated with the activity.
    /// </summary>
    public TState AsyncState { get; init; }
}
