using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class BackgroundProcessStartedEventArgs : BackgroundProcessStartedEventArgsBase<IBackgroundOperation>
    {
        public BackgroundProcessStartedEventArgs([DisallowNull] IBackgroundOperation operation, MessageCode? messageCode, string statusDescription = null)
            : base(operation, messageCode, statusDescription)
        {
        }
    }

    public sealed class BackgroundProcessStartedEventArgs<TState> : BackgroundProcessStartedEventArgsBase<IBackgroundOperation<TState>>, IBackgroundProgressStartedEvent<TState>
    {
        public BackgroundProcessStartedEventArgs([DisallowNull] IBackgroundOperation<TState> operation, MessageCode? messageCode, string statusDescription = null)
            : base(operation, messageCode, statusDescription)
        {
            AsyncState = operation.AsyncState;
        }

        public TState AsyncState { get; }

        public IDisposable Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer) => Operation.Subscribe(observer);
    }
}
