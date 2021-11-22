using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Services
{
    public sealed class BackgroundProcessStartedEventArgs : BackgroundProcessStateEventArgs, IBackgroundProgressStartedEvent
    {
        private readonly IBackgroundOperation _operation;

        public BackgroundProcessStartedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation operation, MessageCode? messageCode)
            : base(source, operation, messageCode)
        {
            _operation = operation;
        }

        public IDisposable Subscribe(IObserver<IBackgroundProgressEvent> observer) => _operation.Subscribe(observer);
    }
}
