using FsInfoCat.AsyncOps;
using System;

namespace FsInfoCat
{
    /// <summary>
    /// Progress information for an asynchronous operation.
    /// </summary>
    /// <remarks>This is roughly based off of the same concept as the PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord">ProgressRecord</see> class.</remarks>
    [Obsolete("Use FsInfoCat.AsyncOps.IBackgroundProgressInfo, instead")]
    public interface IAsyncOperationInfo
    {
        /// <summary>
        /// Uniquely identifies an asynchronous operation.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ActivityId">ProgressRecord.ActivityId</see> property.</remarks>
        Guid ConcurrencyId { get; }

        /// <summary>
        /// Identifies the high-level activity that the asynchronous operation performs.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_Activity">ProgressRecord.Activity</see> property.</remarks>
        ActivityCode? Activity { get; }

        /// <summary>
        /// Gets the status of the background operation.
        /// </summary>
        /// <remarks>This covers the same concept of the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_RecordType">ProgressRecord.RecordType</see> property.</remarks>
        AsyncJobStatus Status { get; }

        /// <summary>
        /// Describes the status of the activity.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_StatusDescription">ProgressRecord.StatusDescription</see> property.</remarks>
        MessageCode? StatusDescription { get; }

        /// <summary>
        /// Describes the operation that is currently taking place.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_CurrentOperation">ProgressRecord.CurrentOperation</see> property.</remarks>
        string CurrentOperation { get; }

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about the asynchronous operation.
        /// </summary>
        object AsyncState { get; }

        /// <summary>
        /// Gets the parent asynchronous operation or <see langword="null"/> if there is no parent operation.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ParentActivityId">ProgressRecord.ParentActivityId</see> property.</remarks>
        IAsyncOperationInfo ParentOperation { get; }
    }

    /// <summary>
    /// Progress information for an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the asynchronous state value.</typeparam>
    [Obsolete("Use FsInfoCat.AsyncOps.IBackgroundProgressInfo, instead")]
    public interface IAsyncOperationInfo<T> : IAsyncOperationInfo
    {
        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about the asynchronous operation.
        /// </summary>
        new T AsyncState { get; }
    }
}
