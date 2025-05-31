using System;

namespace FsInfoCat.Activities;

/// <summary>
/// Describes an event for a timed operational activity.
/// </summary>
public record TimedOperationEvent : OperationEvent, ITimedOperationEvent
{
    /// <summary>
    /// Gets the start time.
    /// </summary>
    /// <value>The date and time when the activity was started started.</value>
    /// <remarks>If <see cref="IActivityStatusInfo.StatusValue"/> is <see cref="ActivityStatus.WaitingToRun"/>, this will be the date and time when this object was instantiated.</remarks>
    public DateTime Started { get; init; }

    /// <summary>
    /// Gets the duration of the activity.
    /// </summary>
    /// <value>The duration of the activity.</value>
    public TimeSpan Duration { get; init; }
}

/// <summary>
/// Describes an event for a timed operational activity that is associated with a user-specified value.
/// </summary>
/// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
public record TimedOperationEvent<TState> : TimedOperationEvent, ITimedOperationEvent<TState>
{
    /// <summary>
    /// Gets the user-defined value that is associated with the operational activity.
    /// </summary>
    public TState AsyncState { get; init; }
}

