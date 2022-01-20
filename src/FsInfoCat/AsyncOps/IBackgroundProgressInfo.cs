using System;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes an asynchronous operation.
    /// </summary>
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundProgressInfo
    {
        /// <summary>
        /// Uniquely identifies the asynchronous operation.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ActivityId">ProgressRecord.ActivityId</see> property.</remarks>
        Guid OperationId { get; }

        /// <summary>
        /// Short description of the high-level activity that the asynchronous operation performs.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_Activity">ProgressRecord.Activity</see> property.</remarks>
        string Activity { get; }

        /// <summary>
        /// Describes the status of the activity.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_StatusDescription">ProgressRecord.StatusDescription</see> property.</remarks>
        string StatusDescription { get; }

        /// <summary>
        /// Describes the specific operation that is currently taking place.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_CurrentOperation">ProgressRecord.CurrentOperation</see> property.</remarks>
        string CurrentOperation { get; }

        /// <summary>
        /// Gets the <see cref="OperationId"/> of the parent asynchronous operation or <see langword="null"/> if there is no parent operation.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ParentActivityId">ProgressRecord.ParentActivityId</see> property.</remarks>
        Guid? ParentId { get; }

        /// <summary>
        /// Gets the job completion percentage value.
        /// </summary>
        /// <value>The job completion percentage value or <see langword="null"/> if not applicable.</value>
        byte? PercentComplete { get; }
    }

    /// <summary>
    /// Describes an asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <seealso cref="IBackgroundProgressInfo" />
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundProgressInfo<TState> : IBackgroundProgressInfo
    {
        /// <summary>
        /// Gets the user-defined object associated with the asynchronous operation.
        /// </summary>
        /// <value>The user-defined object associated with the asynchronous operation.</value>
        TState AsyncState { get; }
    }
}
