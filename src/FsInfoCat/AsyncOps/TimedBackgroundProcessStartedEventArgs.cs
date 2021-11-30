using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    public class TimedBackgroundProcessStartedEventArgs : BackgroundProcessStartedEventArgsBase<ITimedBackgroundOperation>, ITimedBackgroundProgressStartedEvent
    {
        public TimeSpan Duration { get; }

        public TimedBackgroundProcessStartedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] ITimedBackgroundOperation operation, MessageCode? messageCode)
            : base(source, operation, messageCode)
        {
            Duration = operation.Duration;
        }

        public IDisposable Subscribe(IObserver<ITimedBackgroundProgressEvent> observer) => Operation.Subscribe(observer);
    }

    public sealed class TimedBackgroundProcessStartedEventArgs<TState> : BackgroundProcessStartedEventArgsBase<ITimedBackgroundOperation<TState>>, ITimedBackgroundProgressStartedEvent<TState>
    {
        public TimeSpan Duration { get; }

        public TState AsyncState { get; }

        public TimedBackgroundProcessStartedEventArgs([DisallowNull] IBackgroundProgressService source, [DisallowNull] ITimedBackgroundOperation<TState> operation, MessageCode? messageCode)
            : base(source, operation, messageCode)
        {
            Duration = operation.Duration;
            AsyncState = operation.AsyncState;
        }

        public IDisposable Subscribe(IObserver<ITimedBackgroundProgressEvent> observer) => Operation.Subscribe(observer);

        public IDisposable Subscribe(IObserver<IBackgroundProgressEvent<TState>> observer) => Operation.Subscribe(observer);

        public IDisposable Subscribe(IObserver<ITimedBackgroundProgressEvent<TState>> observer) => Operation.Subscribe(observer);
    }
}
