using System;

namespace FsInfoCat.Activities;

/// <summary>
/// Describes an activity lifecycle event.
/// </summary>
public record ActivityEvent : IActivityEvent
{
    /// <summary>
    /// Gets the exception (if any) associated with the event.
    /// </summary>
    /// <value>The <see cref="Exception" /> associated with the event or <see langword="null" /> if there is none.</value>
    public Exception Exception { get; init; }

    /// <summary>
    /// Gets the unique identifier of the described activity.
    /// </summary>
    /// <value>The <see cref="Guid" /> value that is unique to the described activity.</value>
    /// <remarks>This serves the same conceptual purpose as the PowerShell
    /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid">ProgressRecord.ActivityId</see>
    /// property.</remarks>
    public Guid ActivityId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the parent activity.
    /// </summary>
    /// <value>The <see cref="Guid" /> value that is unique to the parent activity or <see langword="null" /> if there is no parent activity.</value>
    /// <remarks>This serves the same conceptual purpose as the PowerShell
    /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.parentactivityid">ProgressRecord.ParentActivityId</see>
    /// property.</remarks>
    public Guid? ParentActivityId { get; init; }

    /// <summary>
    /// Gets the short description of the activity.
    /// </summary>
    /// <value>A <see cref="string" /> that describes the activity.</value>
    /// <remarks>This serves the same conceptual purpose as the PowerShell
    /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activity">ProgressRecord.Activity</see> property and
    /// should never be <see langword="null" /> or <see cref="string.Empty" />.</remarks>
    public string ShortDescription { get; init; }

    /// <summary>
    /// Gets the description of the activity's current status.
    /// </summary>
    /// <value>A <see cref="string" /> that contains a short message describing current status of the activity.</value>
    /// <remarks>This serves the same conceptual purpose as the PowerShell
    /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.statusDescription">ProgressRecord.StatusDescription</see>
    /// property and should never be <see langword="null" /> or <see cref="string.Empty" />.</remarks>
    public string StatusMessage { get; init; }

    /// <summary>
    /// Gets the status message level.
    /// </summary>
    /// <value>The message level value for the associated <see cref="StatusMessage"/>.</value>
    public Model.StatusMessageLevel MessageLevel { get; init; }
}

/// <summary>
/// Describes a lifecycle event for an activity that is associated with a user-specified value.
/// </summary>
/// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
public record ActivityEvent<TState> : ActivityEvent, IActivityEvent<TState>
{
    /// <summary>
    /// Gets the user-defined value that is associated with the activity.
    /// </summary>
    public TState AsyncState { get; init; }
}
