using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a progress reporting object for a <see cref="IAsyncActivity"/>.
    /// </summary>
    /// <seealso cref="IProgress{string}" />
    /// <seealso cref="IProgress{Exception}" />
    /// <seealso cref="IProgress{int}" />
    /// <seealso cref="IOperationInfo" />
    /// <seealso cref="IAsyncActivityProvider" />
    public interface IActivityProgress : IProgress<string>, IProgress<Exception>, IProgress<int>, IOperationInfo, IAsyncActivityProvider
    {
        /// <summary>
        /// Gets the cancellation token for the current asynchronous operation.
        /// </summary>
        /// <value>The <see cref="CancellationToken"/> for the associated <see cref="IAsyncActivity"/>.</value>
        CancellationToken Token { get; }

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/>, <see cref="IOperationInfo.CurrentOperation"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object
        /// and reports a non-fatal operation error.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete"/> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
        /// specified.</param>
        /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning"/> if <see langword="true"/>; otherwise, <see cref="StatusMessageLevel.Error"/> if <see langword="false"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percentComplete"/> is less than <c>-1</c> or greater than <c>100</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IActivityInfo.StatusMessage"/>, <see cref="IOperationInfo.CurrentOperation"/> or <see cref="IOperationInfo.PercentComplete"/> does not change.
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void Report([DisallowNull] Exception error, string statusDescription, string currentOperation, int percentComplete, bool isWarning);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/>, <see cref="IOperationInfo.CurrentOperation"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object
        /// and reports a non-fatal operation <see cref="StatusMessageLevel.Error"/>.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete"/> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
        /// specified.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percentComplete"/> is less than <c>-1</c> or greater than <c>100</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IActivityInfo.StatusMessage"/>, <see cref="IOperationInfo.CurrentOperation"/> or <see cref="IOperationInfo.PercentComplete"/> does not change.
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void Report([DisallowNull] Exception error, string statusDescription, string currentOperation, int percentComplete);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/>, <see cref="IOperationInfo.CurrentOperation"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object
        /// and reports the changed operation status.
        /// </summary>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete"/> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
        /// specified.</param>
        /// <param name="messageLevel">The <see cref="IActivityEvent.MessageLevel"/> value of the event.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percentComplete"/> is less than <c>-1</c> or greater than <c>100</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>If the value of <see cref="IActivityInfo.StatusMessage"/>, <see cref="IOperationInfo.CurrentOperation"/> or <see cref="IOperationInfo.PercentComplete"/> changes, then an activity notification event will be
        /// pushed to reflect the new operational state; otherwise, this will have no effect.
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void Report([DisallowNull] string statusDescription, string currentOperation, int percentComplete, StatusMessageLevel messageLevel);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/>, <see cref="IOperationInfo.CurrentOperation"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object
        /// and reports the changed operation <see cref="StatusMessageLevel.Information"/> status.
        /// </summary>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete"/> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
        /// specified.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percentComplete"/> is less than <c>-1</c> or greater than <c>100</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>If the value of <see cref="IActivityInfo.StatusMessage"/>, <see cref="IOperationInfo.CurrentOperation"/> or <see cref="IOperationInfo.PercentComplete"/> changes, then an activity notification event will be
        /// pushed to reflect the new operational state; otherwise, this will have no effect.
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void Report([DisallowNull] string statusDescription, string currentOperation, int percentComplete);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object and reports a non-fatal operation error.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning"/> if <see langword="true"/>; otherwise, <see cref="StatusMessageLevel.Error"/> if <see langword="false"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IActivityInfo.StatusMessage"/> or <see cref="IOperationInfo.CurrentOperation"/> does not change.
        /// <para>The <see cref="IOperationInfo.CurrentOperation"/> property will be set to a <see cref="string.Empty"/> on this progress object and on the pushed <see cref="IOperationEvent"/>.</para>
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription, string currentOperation, bool isWarning);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object and reports a non-fatal operation <see cref="StatusMessageLevel.Error"/>.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IActivityInfo.StatusMessage"/> or <see cref="IOperationInfo.CurrentOperation"/> does not change.
        /// <para>The <see cref="IOperationInfo.CurrentOperation"/> property will be set to a <see cref="string.Empty"/> on this progress object and on the pushed <see cref="IOperationEvent"/>.</para>
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription, string currentOperation);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object and reports a non-fatal operation error.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete"/> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
        /// specified.</param>
        /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning"/> if <see langword="true"/>; otherwise, <see cref="StatusMessageLevel.Error"/> if <see langword="false"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IActivityInfo.StatusMessage"/> or <see cref="IOperationInfo.PercentComplete"/> does not change.
        /// <para>The <see cref="IOperationInfo.CurrentOperation"/> property will be set to a <see cref="string.Empty"/> on this progress object and on the pushed <see cref="IOperationEvent"/>.</para>
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para></remarks>
        void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription, int percentComplete, bool isWarning);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object and reports a non-fatal operation <see cref="StatusMessageLevel.Error"/>.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete"/> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
        /// specified.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IActivityInfo.StatusMessage"/> or <see cref="IOperationInfo.PercentComplete"/> does not change.
        /// <para>The <see cref="IOperationInfo.CurrentOperation"/> property will be set to a <see cref="string.Empty"/> on this progress object and on the pushed <see cref="IOperationEvent"/>.</para>
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para></remarks>
        void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription, int percentComplete);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/> and <see cref="IOperationInfo.CurrentOperation"/> properties for this progress object and reports the changed operation status.
        /// </summary>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="messageLevel">The <see cref="IActivityEvent.MessageLevel"/> value of the event.</param>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>If the value of <see cref="IActivityInfo.StatusMessage"/> or <see cref="IOperationInfo.CurrentOperation"/> changes, then an activity notification event will be pushed to reflect the new operational state;
        /// otherwise, this will have no effect.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IOperationInfo.PercentComplete"/> value.</para>
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void Report([DisallowNull] string statusDescription, string currentOperation, StatusMessageLevel messageLevel);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/> and <see cref="IOperationInfo.CurrentOperation"/> properties for this progress object and reports the changed operation <see cref="StatusMessageLevel.Information"/> status.
        /// </summary>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>If the value of <see cref="IActivityInfo.StatusMessage"/> or <see cref="IOperationInfo.CurrentOperation"/> changes, then an activity notification event will be pushed to reflect the new operational state;
        /// otherwise, this will have no effect.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IOperationInfo.PercentComplete"/> value.</para>
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void Report([DisallowNull] string statusDescription, string currentOperation);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object and reports a non-fatal operation error.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning"/> if <see langword="true"/>; otherwise, <see cref="StatusMessageLevel.Error"/> if <see langword="false"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IOperationInfo.CurrentOperation"/> does not change.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IOperationInfo.PercentComplete"/>, <see cref="IActivityInfo.StatusMessage"/> values.</para>
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para></remarks>
        void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription, bool isWarning);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object and reports a non-fatal operation <see cref="StatusMessageLevel.Error"/>.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IOperationInfo.CurrentOperation"/> does not change.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IOperationInfo.PercentComplete"/>, <see cref="IActivityInfo.StatusMessage"/> values.</para>
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para></remarks>
        void Report([DisallowNull] Exception error, [DisallowNull] string statusDescription);

        /// <summary>
        /// Updates the <see cref="IOperationInfo.CurrentOperation"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object and reports a non-fatal operation error.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete"/> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
        /// specified.</param>
        /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning"/> if <see langword="true"/>; otherwise, <see cref="StatusMessageLevel.Error"/> if <see langword="false"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percentComplete"/> is less than <c>-1</c> or greater than <c>100</c>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IOperationInfo.CurrentOperation"/> or <see cref="IOperationInfo.PercentComplete"/> does not change.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IActivityInfo.StatusMessage"/> value.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void ReportCurrentOperation([DisallowNull] Exception error, string currentOperation, int percentComplete, bool isWarning);

        /// <summary>
        /// Updates the <see cref="IOperationInfo.CurrentOperation"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object and reports a non-fatal operation <see cref="StatusMessageLevel.Error"/>.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete"/> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
        /// specified.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percentComplete"/> is less than <c>-1</c> or greater than <c>100</c>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IOperationInfo.CurrentOperation"/> or <see cref="IOperationInfo.PercentComplete"/> does not change.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IActivityInfo.StatusMessage"/> value.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void ReportCurrentOperation([DisallowNull] Exception error, string currentOperation, int percentComplete);

        /// <summary>
        /// Updates the <see cref="IOperationInfo.CurrentOperation"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object and reports the changed operation status.
        /// </summary>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete"/> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
        /// specified.</param>
        /// <param name="messageLevel">The <see cref="IActivityEvent.MessageLevel"/> value of the event.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percentComplete"/> is less than <c>-1</c> or greater than <c>100</c>.</exception>
        /// <remarks>If the value of <see cref="IOperationInfo.CurrentOperation"/> or <paramref name="IOperationInfo.PercentComplete"/> changes, then an activity notification event will be pushed to reflect the new operational
        /// state; otherwise, this will have no effect.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IActivityInfo.StatusMessage"/> value.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void ReportCurrentOperation(string currentOperation, int percentComplete, StatusMessageLevel messageLevel);

        /// <summary>
        /// Updates the <see cref="IOperationInfo.CurrentOperation"/> and <see cref="IOperationInfo.PercentComplete"/> properties for this progress object and reports the changed operation <see cref="StatusMessageLevel.Information"/> status.
        /// </summary>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="percentComplete">The new value for <paramref name="IOperationInfo.PercentComplete"/> as value from <c>-1</c> through <c>100</c>, where <c>-1</c> indicates no completion percentage is
        /// specified.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percentComplete"/> is less than <c>-1</c> or greater than <c>100</c>.</exception>
        /// <remarks>If the value of <see cref="IOperationInfo.CurrentOperation"/> or <paramref name="IOperationInfo.PercentComplete"/> changes, then an activity notification event will be pushed to reflect the new operational
        /// state; otherwise, this will have no effect.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IActivityInfo.StatusMessage"/> value.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void ReportCurrentOperation(string currentOperation, int percentComplete);

        /// <summary>
        /// Updates the <see cref="IOperationInfo.CurrentOperation"/> property for this progress object and reports a non-fatal operation error.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning"/> if <see langword="true"/>; otherwise, <see cref="StatusMessageLevel.Error"/> if <see langword="false"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IOperationInfo.CurrentOperation"/> does not change.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IOperationInfo.PercentComplete"/>, <see cref="IActivityInfo.StatusMessage"/> values.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void ReportCurrentOperation([DisallowNull] Exception error, string currentOperation, bool isWarning);

        /// <summary>
        /// Updates the <see cref="IOperationInfo.CurrentOperation"/> property for this progress object and reports a non-fatal operation <see cref="StatusMessageLevel.Error"/>.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        /// <remarks>Calling this method will result in an activity notification event being pushed with the specified <paramref name="error"/> as the <see cref="IActivityEvent.Exception"/>,
        /// even if the value of <see cref="IOperationInfo.CurrentOperation"/> does not change.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IOperationInfo.PercentComplete"/>, <see cref="IActivityInfo.StatusMessage"/> values.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void ReportCurrentOperation([DisallowNull] Exception error, string currentOperation);

        /// <summary>
        /// Updates the <see cref="IActivityInfo.StatusMessage"/> and <see cref="IOperationInfo.CurrentOperation"/> properties for this progress object and reports the changed operation status.
        /// </summary>
        /// <param name="statusDescription">The new value of <see cref="IActivityInfo.StatusMessage"/>.</param>
        /// <param name="messageLevel">The <see cref="IActivityEvent.MessageLevel"/> value of the event.</param>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, <see cref="string.Empty"/>
        /// or contains only <see cref="string.IsNullOrWhiteSpace(string)">white space characters</see>.</exception>
        /// <remarks>If the value of <see cref="IActivityInfo.StatusMessage"/> or <see cref="IOperationInfo.CurrentOperation"/> changes, then an activity notification event will be pushed to reflect the new operational state;
        /// otherwise, this will have no effect.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IOperationInfo.PercentComplete"/> value.</para>
        /// <para>Any white space in <paramref name="statusDescription"/> will normalized before it is applied to <see cref="IActivityInfo.StatusMessage"/>.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void Report([DisallowNull] string statusDescription, StatusMessageLevel messageLevel);

        /// <summary>
        /// Updates the value of updating <see cref="IOperationInfo.CurrentOperation"/> for this progress object and reports the changed operation status.
        /// </summary>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <param name="messageLevel">The <see cref="IActivityEvent.MessageLevel"/> value of the event.</param>
        /// <remarks>If the value of <see cref="IOperationInfo.CurrentOperation"/> changes, then an activity notification event will be pushed to reflect the new operational state; otherwise, this will have no effect.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IOperationInfo.PercentComplete"/>, <see cref="IActivityInfo.StatusMessage"/> values.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void ReportCurrentOperation(string currentOperation, StatusMessageLevel messageLevel);

        /// <summary>
        /// Updates the <see cref="IOperationInfo.CurrentOperation"/> property for this progress object and reports a non-fatal operation error.
        /// </summary>
        /// <param name="error">The non-fatal error that was encountered by the current operation.</param>
        /// <param name="isWarning">Reports a <see cref="StatusMessageLevel.Warning"/> if <see langword="true"/>; otherwise, <see cref="StatusMessageLevel.Error"/> if <see langword="false"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
        void Report([DisallowNull] Exception error, bool isWarning);

        /// <summary>
        /// Updates the value of updating <see cref="IOperationInfo.CurrentOperation"/> for this progress object and reports the changed operation <see cref="StatusMessageLevel.Information"/> status.
        /// </summary>
        /// <param name="currentOperation">The new value for <see cref="IOperationInfo.CurrentOperation"/>.</param>
        /// <remarks>If the value of <see cref="IOperationInfo.CurrentOperation"/> changes, then an activity notification event will be pushed to reflect the new operational state; otherwise, this will have no effect.
        /// <para>The pushed <see cref="IOperationEvent"/> will be populated with the latest <see cref="IOperationInfo.PercentComplete"/>, <see cref="IActivityInfo.StatusMessage"/> values.</para>
        /// <para>If <paramref name="currentOperation"/> is <see langword="null"/> it will converted to a <see cref="string.Empty"/>; otherwise, any extraneous whitespace will be trimmed.</para></remarks>
        void ReportCurrentOperation(string currentOperation);
    }

    /// <summary>
    /// Represents a progress reporting object for a <see cref="IAsyncActivity"/> that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value that is associated with the current asynchronous activity.</typeparam>
    /// <seealso cref="IOperationInfo{TState}" />
    /// <seealso cref="IAsyncActivityProvider" />
    /// <seealso cref="IActivityProgress" />
    public interface IActivityProgress<TState> : IOperationInfo<TState>, IAsyncActivityProvider, IActivityProgress { }
}
