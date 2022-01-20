using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class BackgroundProgressErrorEventArgs : BackgroundProgressEventArgs, IBackgroundOperationErrorEvent
    {
        public Exception Error { get; }

        ErrorCode IBackgroundOperationErrorEvent.Code => Code.ToErrorCode() ?? ErrorCode.Unexpected;

        public BackgroundProgressErrorEventArgs([DisallowNull] IBackgroundProgressInfo progress, [DisallowNull] Exception error, ErrorCode code)
            : base(progress, code.ToMessageCode(MessageCode.UnexpectedError))
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public BackgroundProgressErrorEventArgs([DisallowNull] IBackgroundProgressInfo progress, [DisallowNull] Exception error)
            : this(progress, error, error is AsyncOperationException failureException ? failureException.Code : ErrorCode.Unexpected) { }
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class BackgroundProgressErrorEventArgs<TState> : BackgroundProgressErrorEventArgs, IBackgroundOperationErrorEvent<TState>
    {
        public TState AsyncState { get; }

        public BackgroundProgressErrorEventArgs([DisallowNull] IBackgroundProgressInfo<TState> progress, [DisallowNull] Exception error, ErrorCode code) : base(progress, error, code) => AsyncState = progress.AsyncState;

        public BackgroundProgressErrorEventArgs([DisallowNull] IBackgroundProgressInfo<TState> progress, [DisallowNull] Exception error) : base(progress, error) => AsyncState = progress.AsyncState;
    }
}
