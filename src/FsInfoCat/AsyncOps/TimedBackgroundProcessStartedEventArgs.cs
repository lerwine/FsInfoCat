using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class TimedBackgroundProcessStartedEventArgs : BackgroundProcessStartedEventArgsBase<ITimedBackgroundOperation>, ITimedBackgroundProgressStartedEvent
    {
        public TimeSpan Duration { get; }

        public TimedBackgroundProcessStartedEventArgs([DisallowNull] ITimedBackgroundOperation operation, MessageCode? messageCode, string statusDescription = null)
            : base(operation, messageCode, statusDescription)
        {
            Duration = operation.Duration;
        }

        public IDisposable Subscribe(IObserver<ITimedBackgroundProgressEvent> observer) => Operation.Subscribe(observer);
    }

    public sealed class TimedBackgroundProcessStartedEventArgs<TState> : BackgroundProcessStartedEventArgsBase<ITimedBackgroundOperation<TState>>, ITimedBackgroundProgressStartedEvent<TState>
    {
        public TimeSpan Duration { get; }

        public TState AsyncState { get; }

        public TimedBackgroundProcessStartedEventArgs([DisallowNull] ITimedBackgroundOperation<TState> operation, MessageCode? messageCode, string statusDescription = null)
            : base(operation, messageCode, statusDescription)
        {
            Duration = operation.Duration;
            AsyncState = operation.AsyncState;
        }

        public IDisposable Subscribe(IObserver<ITimedBackgroundProgressEvent> observer) => Operation.Subscribe(observer);

        public IDisposable Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer) => Operation.Subscribe(observer);

        public IDisposable Subscribe(IObserver<ITimedBackgroundProgressEvent<TState>> observer) => Operation.Subscribe(observer);
    }
}
