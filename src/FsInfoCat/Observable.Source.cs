using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public sealed partial class Observable<TNotice>
    {
        // TODO: Document Observable<TNotice>.Source class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public sealed class Source : IDisposable
        {
            public object SyncRoot => Observable._syncRoot;

            public Observable<TNotice> Observable { get; } = new();

            /// <summary>
            /// Sends a push notification of type <typeparamref name="TNotice"/> to all subscribed  <see cref="IObserver{TNotice}" /> objects.
            /// </summary>
            /// <param name="value">The value of the push notification.</param>
            public void RaiseNext(TNotice value) => Observable.RaiseNext(Registration.GetObservers(Observable), value);

            /// <summary>
            /// Sends an error notification to all subscribed  <see cref="IObserver{TNotice}" /> objects.
            /// </summary>
            /// <param name="error">An object that provides additional information about the error.</param>
            /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/></exception>
            public void RaiseError([DisallowNull] Exception error) => Observable.RaiseError(Registration.GetObservers(Observable), error ?? throw new ArgumentNullException(nameof(error)));

            private void Dispose(bool disposing)
            {
                if (!Observable._isDisposed && disposing)
                {
                    Observable._isDisposed = true;
                    Registration.Dispose(Observable);
                }
            }

            // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            // ~Source()
            // {
            //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            //     Dispose(disposing: false);
            // }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
