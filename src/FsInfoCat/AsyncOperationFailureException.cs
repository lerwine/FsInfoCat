using System;
using System.Runtime.Serialization;

namespace FsInfoCat
{

    [Serializable]
    public class AsyncOperationFailureException : Exception, IAsyncOperationInfo
    {
        public string UserMessage { get; }

        public IAsyncOperationInfo AsyncOperation { get; }

        public ErrorCode? ErrorCode { get; }

        AsyncJobStatus IAsyncOperationInfo.Status => AsyncJobStatus.Faulted;

        MessageCode? IAsyncOperationInfo.StatusDescription => ErrorCode?.GetAmbientValue<ErrorCode, MessageCode>();

        ActivityCode? IAsyncOperationInfo.Activity => AsyncOperation?.Activity;

        string IAsyncOperationInfo.CurrentOperation => AsyncOperation?.CurrentOperation;

        object IAsyncOperationInfo.AsyncState => AsyncOperation?.AsyncState;

        public AsyncOperationFailureException() { }

        public AsyncOperationFailureException(string message, string userMessage = null, IAsyncOperationInfo asyncOp = null) : base(message)
        {
            UserMessage = userMessage ?? Message;
            AsyncOperation = asyncOp;
        }

        public AsyncOperationFailureException(string message, ErrorCode errorCode, string userMessage = null, IAsyncOperationInfo asyncOp = null) : this(message, userMessage ?? errorCode.GetDisplayName(), asyncOp)
        {
            ErrorCode = errorCode;
        }

        public AsyncOperationFailureException(string message, string userMessage, Exception inner, IAsyncOperationInfo asyncOp = null) : base(message, (inner is AggregateException a && a.InnerExceptions.Count < 2) ? a.InnerException : inner)
        {
            UserMessage = userMessage ?? Message;
            AsyncOperation = asyncOp;
        }

        public AsyncOperationFailureException(string message, ErrorCode errorCode, string userMessage, Exception inner, IAsyncOperationInfo asyncOp = null) : this(message, userMessage ?? errorCode.GetDisplayName(), inner, asyncOp)
        {
            ErrorCode = errorCode;
        }

        public AsyncOperationFailureException(string message, ErrorCode errorCode, Exception inner, IAsyncOperationInfo asyncOp = null) : this(message, errorCode, null, inner, asyncOp) { }

        protected AsyncOperationFailureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
