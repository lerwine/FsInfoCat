using System;
using System.Diagnostics.CodeAnalysis;
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
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundProgress<TEvent> : IBackgroundProgressInfo, IProgress<TEvent>, IBackgroundProgressFactory
        where TEvent : IBackgroundProgressEvent
    {
        /// <summary>
        /// Gets the camcellation token.
        /// </summary>
        /// <value>The token that is used to communicate background operation cancellation requests.</value>
        CancellationToken Token { get; }

        void ReportCurrentOperation([DisallowNull] string currentOperation, MessageCode code, byte percentComplete);

        void ReportCurrentOperation([DisallowNull] string currentOperation, MessageCode code);

        void ReportCurrentOperation([DisallowNull] string currentOperation, byte percentComplete);

        /// <summary>
        /// Reports the current operation.
        /// </summary>
        /// <param name="currentOperation">The description of the specific operation currently being conducted.
        /// <para>This serves the same conceptual purpose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_CurrentOperation">ProgressRecord.CurrentOperation</see> property.</para></param>
        void ReportCurrentOperation([DisallowNull] string currentOperation);

        void ReportStatusDescription([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode code, byte percentComplete);

        void ReportStatusDescription([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode code);

        void ReportStatusDescription([DisallowNull] string statusDescription, [DisallowNull] string currentOperation, byte percentComplete);

        void ReportStatusDescription([DisallowNull] string statusDescription, MessageCode code, byte percentComplete);

        void ReportStatusDescription([DisallowNull] string statusDescription, MessageCode code);

        void ReportStatusDescription([DisallowNull] string statusDescription, byte percentComplete);

        /// <summary>
        /// Reports the background operation status and operation description.
        /// </summary>
        /// <param name="statusDescription">The asynchronous operation status description.
        /// <para>This serves the same conceptual purpose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_StatusDescription">ProgressRecord.StatusDescription</see> property.</para></param>
        /// <param name="currentOperation">The description of the specific operation currently being conducted.
        /// <para>This serves the same conceptual purpose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_CurrentOperation">ProgressRecord.CurrentOperation</see> property.</para></param>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/> or contains only white space.</exception>
        void ReportStatusDescription([DisallowNull] string statusDescription, [DisallowNull] string currentOperation);

        /// <summary>
        /// Reports the background operation status.
        /// </summary>
        /// <param name="statusDescription">The asynchronous operation status description.
        /// <para>This serves the same conceptual purpose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_StatusDescription">ProgressRecord.StatusDescription</see> property.</para></param>
        void ReportStatusDescription([DisallowNull] string statusDescription);

        void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode code, byte percentComplete);

        void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, MessageCode code);

        void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, [DisallowNull] string currentOperation, byte percentComplete);

        void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, MessageCode code, byte percentComplete);

        void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, MessageCode code);

        void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, byte percentComplete);

        void ReportException([DisallowNull] Exception exception, MessageCode code, byte percentComplete);

        void ReportException([DisallowNull] Exception exception, MessageCode code);

        void ReportException([DisallowNull] Exception exception, byte percentComplete);

        void ReportPercentComplete(byte percentComplete);

        /// <summary>
        /// Reports a non-fatal asynchronous operation exception.
        /// </summary>
        /// <param name="exception">The exception to report.</param>
        /// <param name="statusDescription">The asynchronous operation status description.
        /// <para>This serves the same conceptual purpose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_StatusDescription">ProgressRecord.StatusDescription</see> property.</para></param>
        /// <param name="currentOperation">The description of the specific operation currently being conducted.
        /// <para>This serves the same conceptual purpose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_CurrentOperation">ProgressRecord.CurrentOperation</see> property.</para></param>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/> or contains only white space.</exception>
        void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription, [DisallowNull] string currentOperation);

        /// <summary>
        /// Reports a non-fatal asynchronous operation exception.
        /// </summary>
        /// <param name="exception">The exception to report.</param>
        /// <param name="statusDescription">The asynchronous operation status description.
        /// <para>This serves the same conceptual purpose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_StatusDescription">ProgressRecord.StatusDescription</see> property.</para></param>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/> or contains only white space.</exception>
        void ReportException([DisallowNull] Exception exception, [DisallowNull] string statusDescription);

        /// <summary>
        /// Reports a non-fatal asynchronous operation exception.
        /// </summary>
        /// <param name="exception">The exception to report.</param>
        void ReportException([DisallowNull] Exception exception);
    }

    /// <summary>
    /// Reports progress information for an asynchronous operation.
    /// </summary>
    /// <typeparam name="TState">The type of user-defined object associated with the asynchronous operation.</typeparam>
    /// <typeparam name="TEvent">The type of event that gets pushed to the corresponding <see cref="IBackgroundOperation{TState}"/>.</typeparam>
    /// <seealso cref="IBackgroundProgress{TEvent}" />
    /// <seealso cref="IBackgroundProgressInfo{TState}" />
    /// <seealso cref="IBackgroundProgressEvent{TState}" />
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBackgroundProgress<TState, TEvent> : IBackgroundProgress<TEvent>, IBackgroundProgressInfo<TState>
        where TEvent : IBackgroundProgressEvent<TState>
    {
    }
}
