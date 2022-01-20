using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public abstract class BackgroundProcessStartedEventArgsBase<TOperation> : BackgroundProcessStateEventArgs, IBackgroundProgressStartedEvent
        where TOperation : IBackgroundOperation
    {
        protected TOperation Operation { get; }

        public BackgroundProcessStartedEventArgsBase([DisallowNull] TOperation operation, MessageCode? messageCode, string statusDescription = null)
            : base(operation, messageCode, statusDescription)
        {
            Operation = operation;
        }

        public bool IsCancellationRequested => Operation.IsCancellationRequested;

        public void Cancel() => Operation.Cancel();

        public void Cancel(bool throwOnFirstException) => Operation.Cancel(throwOnFirstException);

        public void CancelAfter(int millisecondsDelay) => Operation.CancelAfter(millisecondsDelay);

        public void CancelAfter(TimeSpan delay) => Operation.CancelAfter(delay);

        public IDisposable Subscribe(IObserver<IBackgroundProgressEvent> observer) => Operation.Subscribe(observer);
    }
}
