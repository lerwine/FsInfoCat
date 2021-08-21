using System;
using System.Runtime.Serialization;

namespace FsInfoCat
{
    [Serializable]
    public class AsyncOperationFailureException : Exception
    {
        public string UserMessage { get; }

        public ErrorCode? ErrorCode { get; }

        public AsyncOperationFailureException() { }

        public AsyncOperationFailureException(string message, string userMessage = null) : base(message)
        {
            UserMessage = userMessage ?? Message;
        }

        public AsyncOperationFailureException(string message, ErrorCode errorCode, string userMessage = null) : this(message, userMessage)
        {
            ErrorCode = errorCode;
        }

        public AsyncOperationFailureException(string message, string userMessage, Exception inner) : base(message, inner)
        {
            UserMessage = userMessage ?? Message;
        }

        public AsyncOperationFailureException(string message, ErrorCode errorCode, string userMessage, Exception inner) : this(message, userMessage, inner)
        {
            ErrorCode = errorCode;
        }

        public AsyncOperationFailureException(string message, ErrorCode errorCode, Exception inner) : this(message, errorCode, null, inner) { }

        protected AsyncOperationFailureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
