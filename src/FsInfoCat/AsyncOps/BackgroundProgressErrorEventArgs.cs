using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class BackgroundProgressErrorEventArgs : BackgroundProgressEventArgs, IBackgroundOperationErrorEvent
    {
        public Exception Error { get; }

        MessageCode IBackgroundOperationErrorEvent.Code => Code??MessageCode.UnexpectedError;

        public BackgroundProgressErrorEventArgs([DisallowNull] IBackgroundProgressInfo progress, [DisallowNull] Exception error, ErrorCode code)
            : base(progress, code.ToMessageCode())
        {
            Error=error??throw new ArgumentNullException(nameof(error));
        }

        public BackgroundProgressErrorEventArgs([DisallowNull] IBackgroundProgressInfo progress, [DisallowNull] Exception error)
            : this(progress, error, error is AsyncOperationFailureException failureException ? failureException.ErrorCode??ErrorCode.Unexpected : ErrorCode.Unexpected) { }
    }

    public class BackgroundProgressErrorEventArgs<T> : BackgroundProgressErrorEventArgs, IBackgroundOperationErrorEvent<T>
    {
        public T AsyncState { get; }

        public BackgroundProgressErrorEventArgs([DisallowNull] IBackgroundProgressInfo<T> progress, [DisallowNull] Exception error, ErrorCode code) : base(progress, error, code) => AsyncState=progress.AsyncState;

        public BackgroundProgressErrorEventArgs([DisallowNull] IBackgroundProgressInfo<T> progress, [DisallowNull] Exception error) : base(progress, error) => AsyncState=progress.AsyncState;
    }
}
