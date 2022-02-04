using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents information about the state of an activity.
    /// </summary>
    public interface IActivityInfo
    {
        /// <summary>
        /// Gets the unique identifier of the described activity.
        /// </summary>
        /// <value>The <see cref="Guid" /> value that is unique to the described activity.</value>
        /// <remarks>This serves the same conceptual puropose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid" target="_blank">ProgressRecord.ActivityId</a> property.</remarks>
        Guid ActivityId { get; }

        /// <summary>
        /// Gets the unique identifier of the parent activity.
        /// </summary>
        /// <value>The <see cref="Guid" /> value that is unique to the parent activity or <see langword="null" /> if there is no parent activity.</value>
        /// <remarks>This serves the same conceptual puropose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.parentactivityid" target="_blank">ProgressRecord.ParentActivityId</a> property.</remarks>
        Guid? ParentActivityId { get; }

        /// <summary>
        /// Gets the short description of the activity.
        /// </summary>
        /// <remarks>This should never be <see langword="null" /> or <see cref="string.Empty" />.</remarks>
        /// <value>A <see cref="string" /> that describes the activity.</value>
        /// <remarks>This serves the same conceptual puropose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activity" target="_blank">ProgressRecord.Activity</a> property.</remarks>
        string ShortDescription { get; }

        /// <summary>
        /// Gets the description of the activity's current status.
        /// </summary>
        /// <remarks>This should never be <see langword="null" /> or <see cref="string.Empty" />.</remarks>
        /// <value>A <see cref="string" /> that contains a short message describing current status of the activity.</value>
        /// <remarks>This serves the same conceptual puropose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.statusDescription" target="_blank">ProgressRecord.StatusDescription</a> property.</remarks>
        string StatusMessage { get; }
    }

    /// <summary>
    /// Represents information about the state of an activity that is associated with a user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this activity.</typeparam>
    /// <seealso cref="IActivityInfo" />
    public interface IActivityInfo<TState> : IActivityInfo
    {
        /// <summary>
        /// Gets the user-defined value.
        /// </summary>
        /// <value>The user-defined vaue that is associated with the activity.</value>
        TState AsyncState { get; }
    }
}
