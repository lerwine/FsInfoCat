using System;

namespace FsInfoCat.BgOps
{
    /// <summary>
    /// Represents status information for an asynchronous operation.
    /// </summary>
    public interface IAsyncOpInfo
    {
        /// <summary>
        /// Uniquely identifies an asynchronous operation.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ActivityId">ProgressRecord.ActivityId</see> property.</remarks>
        Guid Id { get; }

        /// <summary>
        /// Identifies the high-level activity that the asynchronous operation performs.
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
        /// Describes the operation that is currently taking place.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_CurrentOperation">ProgressRecord.CurrentOperation</see> property.</remarks>
        string CurrentOperation { get; }

        /// <summary>
        /// Gets the parent asynchronous operation or <see langword="null"/> if there is no parent operation.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ParentActivityId">ProgressRecord.ParentActivityId</see> property.</remarks>
        IAsyncOpInfo ParentOperation { get; }
    }

    /// <summary>
    /// Represents status information for an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
    public interface IAsyncOpInfo<T> : IAsyncOpInfo
    {
        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about the asynchronous operation.
        /// </summary>
        T AsyncState { get; }
    }
}
