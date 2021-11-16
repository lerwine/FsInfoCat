using System;
using System.Threading;

namespace FsInfoCat.BgOps
{
    /// <summary>
    /// Reports asynchronous progress information.
    /// </summary>
    public interface IAsyncOpProgress : IAsyncOpInfo, IProgress<string>, IProgress<Exception>, IAsyncOpFactory
    {
        CancellationToken Token { get; }

        /// <summary>
        /// Gets the parent asynchronous operation or <see langword="null"/> if there is no parent operation.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ParentActivityId">ProgressRecord.ParentActivityId</see> property.</remarks>
        new IAsyncOpEventArgs ParentOperation { get; }

        /// <summary>
        /// Updates the current status description and current operation and raises the <see cref="IAsyncOpEventArgs"/> status update event.
        /// </summary>
        /// <param name="statusDescription">The value for the <see cref="IAsyncOpInfo.StatusDescription"/> property.</param>
        /// <param name="currentOperation">The value for the <see cref="IAsyncOpInfo.CurrentOperation"/> property.</param>
        /// <param name="exception">The value for the <see cref="IAsyncOpEventArgs.Exception"/> property.</param>
        void ReportStatus(string statusDescription, string currentOperation, Exception exception);

        /// <summary>
        /// Updates the current status description and current operation and raises the <see cref="IAsyncOpEventArgs"/> status update event.
        /// </summary>
        /// <param name="statusDescription">The value for the <see cref="IAsyncOpInfo.StatusDescription"/> property.</param>
        /// <param name="currentOperation">The value for the <see cref="IAsyncOpInfo.CurrentOperation"/> property.</param>
        void ReportStatus(string statusDescription, string currentOperation);
        /// <summary>
        /// Updates the current status description, sets the current operation value to an empty string, and raises the <see cref="IAsyncOpEventArgs"/> status update event.
        /// </summary>
        /// <param name="statusMessage">The new value for the <see cref="IAsyncOpInfo.StatusDescription"/> property.</param>
        void ReportStatus(string statusDescription);

        /// <summary>
        /// Updates the current status description, sets the current operation value to an empty string, and raises the <see cref="IAsyncOpEventArgs"/> status update event.
        /// </summary
        /// <param name="currentOperation">The value for the <see cref="IAsyncOpInfo.CurrentOperation"/> property.</param>
        /// <param name="exception">The value for the <see cref="IAsyncOpEventArgs.Exception"/> property.</param>
        void Report(string currentOperation, Exception exception);

    }

    /// <summary>
    /// Reports asynchronous progress information.
    /// </summary>
    /// <typeparam name="T">The type of user-defined object that qualifies or contains information about the asynchronous operation.</typeparam>
    public interface IAsyncOpProgress<T> : IAsyncOpProgress, IAsyncOpInfo<T>
    {
    }
}
