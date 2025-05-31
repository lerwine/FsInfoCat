namespace FsInfoCat.Activities;

/// <summary>
/// Describes an operational activity event.
/// </summary>
public record OperationEvent : ActivityEvent, IOperationEvent
{
    /// <summary>
    /// Gets the value that indicates the lifecycle status of the activity.
    /// </summary>
    public ActivityStatus StatusValue { get; init; }

    /// <summary>
    /// Gets the description of the current operation of the many required to accomplish the activity.
    /// </summary>
    /// <value>The description of the current operation being performed or <see cref="string.Empty" /> if no operation has been started or no operation description has been
    /// provided.</value>
    /// <remarks>This serves the same conceptual purpose as the
    /// PowerShell <see href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.currentoperation">ProgressRecord.CurrentOperation</see>
    /// property and should never be <see langword="null" />.</remarks>
    public string CurrentOperation { get; init; }

    /// <summary>
    /// Gets and sets the estimate of the percentage of total work for the activity that is completed.
    /// </summary>
    /// <value>The estimated percentage completion value from <c>0</c> to <c>100</c> or <c>-1</c> to indicate that the percentage completed should not be displayed.</value>
    /// <remarks>This serves the same conceptual purpose as the PowerShell
    /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.percentcomplete">ProgressRecord.ParentActivityId</see>
    /// property.</remarks>
    public int PercentComplete { get; init; }
}

/// <summary>
/// Describes an event for an operational activity that is associated with a user-specified value.
/// </summary>
/// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
public record OperationEvent<TState> : OperationEvent, IOperationEvent<TState>
{
    /// <summary>
    /// Gets the user-defined value that is associated with the activity.
    /// </summary>
    public TState AsyncState { get; init; }
}
