using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Represents an exception within the context of an asynchronous operation.
    /// </summary>
    /// <seealso cref="Exception"/>
    /// <seealso cref="IBackgroundOperationErrorEvent"/>
    [Serializable]
    [Obsolete("Use FsInfoCat.Activities.ActivityException, instead.")]
    public class AsyncOperationException : Exception, IBackgroundOperationErrorEvent
    {
        /// <summary>
        /// Gets the error code associated with the exception.
        /// </summary>
        /// <value>The code.</value>
        public ErrorCode Code { get; }

        /// <summary>
        /// Gets the unique identifier of the asynchronous operation.
        /// </summary>
        /// <value>The <see cref="Guid"/> that uniquely identifies the <see cref="IBackgroundOperation"/> where the exception was thrown.</value>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ActivityId">ProgressRecord.ActivityId</see> property.</remarks>
        public Guid OperationId { get; }

        /// <summary>
        /// Short description of the high-level activity for the asynchronous operation.
        /// </summary>
        /// <value>The short description of the high-level activity that the asynchronous operation was performing.</value>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_Activity">ProgressRecord.Activity</see> property.</remarks>
        public string Activity { get; }

        /// <summary>
        /// Describes the specific operation for the asynchronous operation.
        /// </summary>
        /// <value>Describes the operation that was being performed when the exception was thrown.</value>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_CurrentOperation">ProgressRecord.CurrentOperation</see> property.</remarks>
        public string CurrentOperation { get; }

        /// <summary>
        /// Gets the <see cref="IBackgroundOperation.OperationId" /> of the parent asynchronous operation.
        /// </summary>
        /// <value>The <see cref="IBackgroundOperation.OperationId" /> of the parent asynchronous operation or <see langword="null" /> if there was no parent operation.</value>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ParentActivityId">ProgressRecord.ParentActivityId</see> property.</remarks>
        public Guid? ParentId { get; }

        /// <summary>
        /// Gets the job completion percentage value.
        /// </summary>
        /// <value>The job completion percentage value at the time the exception was thrown or <see langword="null" /> if not applicable.</value>
        public byte? PercentComplete { get; }

        MessageCode? IBackgroundProgressEvent.Code => Code.ToMessageCode(MessageCode.UnexpectedError);

        string IBackgroundProgressInfo.StatusDescription => Message;

        Exception IBackgroundOperationErrorOptEvent.Error => this;

        public AsyncOperationException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncOperationException"/> class.
        /// </summary>
        /// <param name="progressInfo">The progress information representing the current progress at the time the exception occurred.</param>
        /// <param name="code">The error code to associate with the exception.</param>
        /// <param name="statusMessage">The status message describing the exception.</param>
        /// <exception cref="ArgumentNullException"><paramref name="progressInfo"/> was null.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusMessage"/> was null or contained only whitespace.</exception>
        public AsyncOperationException([DisallowNull] IBackgroundProgressInfo progressInfo, ErrorCode code, [DisallowNull] string statusMessage) : base(statusMessage)
        {
            if (progressInfo is null)
                throw new ArgumentNullException(nameof(progressInfo));
            if (string.IsNullOrWhiteSpace(statusMessage))
                throw new ArgumentException($"'{nameof(statusMessage)}' cannot be null or whitespace.", nameof(statusMessage));
            Code = code;
            OperationId = progressInfo.OperationId;
            Activity = progressInfo.Activity;
            CurrentOperation = progressInfo.CurrentOperation;
            ParentId = progressInfo.ParentId;
            PercentComplete = progressInfo.PercentComplete;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncOperationException"/> class.
        /// </summary>
        /// <param name="progressInfo">The progress information representing the current progress at the time the exception occurred.</param>
        /// <param name="code">The error code to associate with the exception.</param>
        /// <param name="statusMessage">The status message describing the exception.</param>
        /// <param name="inner">The actual exception that was thrown.</param>
        /// <exception cref="ArgumentNullException"><paramref name="progressInfo"/> was null.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusMessage"/> was null or contained only whitespace.</exception>
        public AsyncOperationException([DisallowNull] IBackgroundProgressInfo progressInfo, ErrorCode code, [DisallowNull] string statusMessage, Exception inner) : base(statusMessage, inner)
        {
            if (progressInfo is null)
                throw new ArgumentNullException(nameof(progressInfo));
            if (string.IsNullOrWhiteSpace(statusMessage))
                throw new ArgumentException($"'{nameof(statusMessage)}' cannot be null or whitespace.", nameof(statusMessage));
            Code = code;
            OperationId = progressInfo.OperationId;
            Activity = progressInfo.Activity;
            CurrentOperation = progressInfo.CurrentOperation;
            ParentId = progressInfo.ParentId;
            PercentComplete = progressInfo.PercentComplete;
        }

        protected AsyncOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Code = (ErrorCode)info.GetInt32(nameof(Code));
#pragma warning disable CS8605 // Unboxing a possibly null value.
            OperationId = (Guid)info.GetValue(nameof(OperationId), typeof(Guid));
#pragma warning restore CS8605 // Unboxing a possibly null value.
            Activity = info.GetString(nameof(Activity));
            CurrentOperation = info.GetString(nameof(CurrentOperation));
            ParentId = (Guid?)info.GetValue(nameof(ParentId), typeof(Guid?));
            PercentComplete = (byte?)info.GetValue(nameof(PercentComplete), typeof(byte?));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Code), (int)Code);
            info.AddValue(nameof(OperationId), OperationId, typeof(Guid));
            info.AddValue(nameof(Activity), Activity);
            info.AddValue(nameof(CurrentOperation), CurrentOperation);
            info.AddValue(nameof(ParentId), ParentId, typeof(Guid?));
            info.AddValue(nameof(PercentComplete), PercentComplete, typeof(byte?));
        }
    }
}
