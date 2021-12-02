using System;

namespace FsInfoCat.BgOps
{
    /// <summary>
    /// Asynchronous operation event arguments.
    /// </summary>
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncOpEventArgs : IAsyncOpStatus
    {
        /// <summary>
        /// Gets the parent asynchronous operation or <see langword="null"/> if there is no parent operation.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ParentActivityId">ProgressRecord.ParentActivityId</see> property.</remarks>
        new IAsyncOpEventArgs ParentOperation { get; }

        /// <summary>
        /// Exception encountered in asynchyonrous operation or <see langword="null"/> if there is no exception to report.
        /// </summary>
        Exception Exception { get; }
    }

    /// <summary>
    /// Asynchronous operation event arguments.
    /// </summary>
    /// <typeparam name="T">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncOpEventArgs<T> : IAsyncOpEventArgs, IAsyncOpStatus<T>
    {
    }
}
