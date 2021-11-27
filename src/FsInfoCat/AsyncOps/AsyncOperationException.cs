using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace FsInfoCat.AsyncOps
{
    [Serializable]
    public class AsyncOperationException : Exception, IBackgroundOperationErrorEvent
    {
        public ErrorCode Code { get; }

        public Guid OperationId { get; }

        public string Activity { get; }

        public string CurrentOperation { get; }

        public Guid? ParentId { get; }

        MessageCode? IBackgroundProgressEvent.Code => Code.ToMessageCode();

        MessageCode IBackgroundOperationErrorEvent.Code => Code.ToMessageCode();

        string IBackgroundProgressInfo.StatusDescription => Message;

        Exception IBackgroundOperationErrorOptEvent.Error => this;

        public AsyncOperationException() { }

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
        }

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
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Code), (int)Code);
            info.AddValue(nameof(OperationId), OperationId, typeof(Guid));
            info.AddValue(nameof(Activity), Activity);
            info.AddValue(nameof(CurrentOperation), CurrentOperation);
            info.AddValue(nameof(ParentId), ParentId, typeof(Guid?));
        }
    }
}
