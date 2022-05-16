using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    /// <summary>
    /// Manages push-based notification subscriptions.
    /// </summary>
    /// <typeparam name="TNotice">The type of the notification information.</typeparam>
    /// <seealso cref="IObservable{TNotice}" />
    /// <seealso cref="IDisposable" />
    /// <remarks>This calls <see cref="IObserver{TNotice}.OnCompleted"/> on all subscribed <see cref="IObserver{TNotice}" /> objects when this object is disposed.</remarks>
    public sealed partial class Observable<TNotice> : IObservable<TNotice>
    {
        private readonly object _syncRoot = new();
        private Registration _first;
        private Registration _last;
        private bool _isDisposed;

        /// <summary>
        /// Determines whether the specified observer is subscribed to receive push notifications.
        /// </summary>
        /// <param name="observer">The target observer.</param>
        /// <returns><c>true</c> if the specified <see cref="IObserver{TNotice}" /> is subscribed to receive push notifications of type <typeparamref name="TNotice"/>;
        /// otherwise, <c>false</c>.</returns>
        public bool IsSubscribed(IObserver<TNotice> observer) => observer is not null && Registration.IsSubscribed(this, observer);

        /// <summary>
        /// Notifies this provider that an observer is to receive notifications.
        /// </summary>
        /// <param name="observer">The object that is to receive notifications of type <typeparamref name="TNotice"/>.</param>
        /// <returns>A reference to an interface that allows observers to stop receiving notifications before this provider has finished sending them.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public IDisposable Subscribe([DisallowNull] IObserver<TNotice> observer) => Registration.Subscribe(this, observer ?? throw new ArgumentNullException(nameof(observer)));

        private void RaiseNext([DisallowNull] Queue<IObserver<TNotice>> queue, TNotice value)
        {
            if (queue.TryDequeue(out IObserver<TNotice> observer))
                try { observer.OnNext(value); }
                finally { RaiseNext(queue, value); }
        }

        private void RaiseError(Queue<IObserver<TNotice>> queue, Exception exception)
        {
            if (queue.TryDequeue(out IObserver<TNotice> observer))
                try { observer.OnError(exception); }
                finally { RaiseError(queue, exception); }
        }
    }
}
