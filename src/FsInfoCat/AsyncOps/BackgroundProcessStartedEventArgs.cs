using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class BackgroundProcessStartedEventArgs : BackgroundProcessStartedEventArgsBase<IBackgroundOperation>
    {
        public BackgroundProcessStartedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation operation, MessageCode? messageCode)
            : base(source, operation, messageCode)
        {
        }
    }

    public sealed class BackgroundProcessStartedEventArgs<TState> : BackgroundProcessStartedEventArgsBase<IBackgroundOperation<TState>>, IBackgroundProgressStartedEvent<TState>
    {
        public BackgroundProcessStartedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] IBackgroundOperation<TState> operation, MessageCode? messageCode)
            : base(source, operation, messageCode)
        {
            AsyncState = operation.AsyncState;
        }

        public TState AsyncState { get; }

        public IDisposable Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer) => Operation.Subscribe(observer);
    }
}
