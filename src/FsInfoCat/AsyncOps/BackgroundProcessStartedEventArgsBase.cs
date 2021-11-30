using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public abstract class BackgroundProcessStartedEventArgsBase<TOperation> : BackgroundProcessStateEventArgs, IBackgroundProgressStartedEvent
        where TOperation : IBackgroundOperation
    {
        protected TOperation Operation { get; }

        public BackgroundProcessStartedEventArgsBase([DisallowNull] IBackgroundProgressService source, [DisallowNull] TOperation operation, MessageCode? messageCode)
            : base(source, operation, messageCode)
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
