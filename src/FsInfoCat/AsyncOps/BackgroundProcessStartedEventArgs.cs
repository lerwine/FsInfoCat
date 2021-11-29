using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public sealed class BackgroundProcessStartedEventArgs : BackgroundProcessStateEventArgs, IBackgroundProgressStartedEvent
    {
        private readonly IBackgroundOperation _operation;

        public BackgroundProcessStartedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation operation, MessageCode? messageCode)
            : base(source, operation, messageCode)
        {
            _operation = operation;
        }

        public bool IsCancellationRequested => _operation.IsCancellationRequested;

        public void Cancel() => _operation.Cancel();

        public void Cancel(bool throwOnFirstException) => _operation.Cancel(throwOnFirstException);

        public void CancelAfter(int millisecondsDelay) => _operation.CancelAfter(millisecondsDelay);

        public void CancelAfter(TimeSpan delay) => _operation.CancelAfter(delay);

        public IDisposable Subscribe(IObserver<IBackgroundProgressEvent> observer) => _operation.Subscribe(observer);
    }
}
