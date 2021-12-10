using System;
using System.Threading;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Reports progress information for an asynchronous operation.
    /// </summary>
    /// <typeparam name="TEvent">The type of event that gets pushed to the corresponding <see cref="IBackgroundOperation"/>.</typeparam>
    /// <seealso cref="IBackgroundProgressInfo" />
    /// <seealso cref="IProgress{TEvent}" />
    /// <seealso cref="IBackgroundProgressFactory" />
    /// <seealso cref="IBackgroundProgressEvent" />
    public interface IBackgroundProgress<TEvent> : IBackgroundProgressInfo, IProgress<TEvent>, IBackgroundProgressFactory
        where TEvent : IBackgroundProgressEvent
    {
        /// <summary>
        /// Gets the camcellation token.
        /// </summary>
        /// <value>The token that is used to communicate background operation cancellation requests.</value>
        CancellationToken Token { get; }

        void ReportCurrentOperation(string currentOperation, MessageCode code, int percentComplete);

        void ReportCurrentOperation(string currentOperation, MessageCode code);

        void ReportCurrentOperation(string currentOperation, int percentComplete);

        /// <summary>
        /// Reports the current operation.
        /// </summary>
        /// <param name="currentOperation">The description of the specific operation currently being conducted.
        /// <para>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_CurrentOperation">ProgressRecord.CurrentOperation</see> property.</para></param>
        void ReportCurrentOperation(string currentOperation);

        void ReportStatusDescription(string statusDescription, string currentOperation, MessageCode code, int percentComplete);

        void ReportStatusDescription(string statusDescription, string currentOperation, MessageCode code);

        void ReportStatusDescription(string statusDescription, string currentOperation, int percentComplete);

        void ReportStatusDescription(string statusDescription, MessageCode code, int percentComplete);

        void ReportStatusDescription(string statusDescription, MessageCode code);

        void ReportStatusDescription(string statusDescription, int percentComplete);

        /// <summary>
        /// Reports the background operation status and operation description.
        /// </summary>
        /// <param name="statusDescription">The asynchronous operation status description.
        /// <para>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_StatusDescription">ProgressRecord.StatusDescription</see> property.</para></param>
        /// <param name="currentOperation">The description of the specific operation currently being conducted.
        /// <para>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_CurrentOperation">ProgressRecord.CurrentOperation</see> property.</para></param>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/> or contains only white space.</exception>
        void ReportStatusDescription(string statusDescription, string currentOperation);

        /// <summary>
        /// Reports the background operation status.
        /// </summary>
        /// <param name="statusDescription">The asynchronous operation status description.
        /// <para>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_StatusDescription">ProgressRecord.StatusDescription</see> property.</para></param>
        void ReportStatusDescription(string statusDescription);

        void ReportException(Exception exception, string statusDescription, string currentOperation, MessageCode code, int percentComplete);

        void ReportException(Exception exception, string statusDescription, string currentOperation, MessageCode code);

        void ReportException(Exception exception, string statusDescription, string currentOperation, int percentComplete);

        void ReportException(Exception exception, string statusDescription, MessageCode code, int percentComplete);

        void ReportException(Exception exception, string statusDescription, MessageCode code);

        void ReportException(Exception exception, string statusDescription, int percentComplete);

        void ReportException(Exception exception, MessageCode code, int percentComplete);

        void ReportException(Exception exception, MessageCode code);

        void ReportException(Exception exception, int percentComplete);

        /// <summary>
        /// Reports a non-fatal asynchronous operation exception.
        /// </summary>
        /// <param name="exception">The exception to report.</param>
        /// <param name="statusDescription">The asynchronous operation status description.
        /// <para>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_StatusDescription">ProgressRecord.StatusDescription</see> property.</para></param>
        /// <param name="currentOperation">The description of the specific operation currently being conducted.
        /// <para>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_CurrentOperation">ProgressRecord.CurrentOperation</see> property.</para></param>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/> or contains only white space.</exception>
        void ReportException(Exception exception, string statusDescription, string currentOperation);

        /// <summary>
        /// Reports a non-fatal asynchronous operation exception.
        /// </summary>
        /// <param name="exception">The exception to report.</param>
        /// <param name="statusDescription">The asynchronous operation status description.
        /// <para>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_StatusDescription">ProgressRecord.StatusDescription</see> property.</para></param>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/> or contains only white space.</exception>
        void ReportException(Exception exception, string statusDescription);

        /// <summary>
        /// Reports a non-fatal asynchronous operation exception.
        /// </summary>
        /// <param name="exception">The exception to report.</param>
        void ReportException(Exception exception);
    }

    /// <summary>
    /// Reports progress information for an asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <typeparam name="TEvent">The type of event that gets pushed to the corresponding <see cref="IBackgroundOperation{TState}"/>.</typeparam>
    /// <seealso cref="IBackgroundProgress{TEvent}" />
    /// <seealso cref="IBackgroundProgressInfo{TState}" />
    /// <seealso cref="IBackgroundProgressEvent{TState}" />
    public interface IBackgroundProgress<TState, TEvent> : IBackgroundProgress<TEvent>, IBackgroundProgressInfo<TState>
        where TEvent : IBackgroundProgressEvent<TState>
    {
    }
}
