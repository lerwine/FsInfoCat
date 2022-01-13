using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public sealed partial class Observable<TNotice>
    {
        sealed class Registration : IDisposable
        {
            private Observable<TNotice> _target;
            private Registration _previous;
            private Registration _next;

            private Registration(Observable<TNotice> target, IObserver<TNotice> observer)
            {
                _previous = (_target = (target ?? throw new ArgumentNullException(nameof(target))))._last;
                Observer = new(observer ?? throw new ArgumentNullException(nameof(observer)));
            }

            internal static bool IsSubscribed(Observable<TNotice> target, IObserver<TNotice> observer)
            {
                lock ((target ?? throw new ArgumentNullException(nameof(target)))._syncRoot)
                {
                    if (target._isDisposed)
                        throw new ObjectDisposedException(target.GetType().FullName);

                    Registration r = target._first;
                    while (r is not null)
                    {
                        if (r.Observer.TryGetTarget(out IObserver<TNotice> o))
                        {
                            if (ReferenceEquals(o, observer))
                                return true;
                        }
                        else
                        {
                            Registration n = r._next;
                            if (n is null)
                            {
                                if ((target._last = r._previous) is null)
                                    target._first = null;
                                else
                                {
                                    r._previous = null;
                                    target._last._next = null;
                                }
                            }
                            else
                            {
                                if ((n._previous = r._previous) is null)
                                    target._first = n;
                                else
                                {
                                    n._previous._next = n;
                                    r._previous = null;
                                }
                                r._next = null;
                            }
                            r._target = null;
                            r = n;
                        }
                    }
                }
                return false;
            }

            internal WeakReference<IObserver<TNotice>> Observer { get; }

            internal static Queue<IObserver<TNotice>> GetObservers(Observable<TNotice> target)
            {
                Queue<IObserver<TNotice>> observers = new();
                lock (target._syncRoot)
                {
                    if (target._isDisposed)
                        throw new InvalidOperationException();
                    Registration r = target._first;
                    while (r is not null)
                    {
                        if (r.Observer.TryGetTarget(out IObserver<TNotice> observer))
                        {
                            observers.Enqueue(observer);
                            r = r._next;
                        }
                        else
                        {
                            Registration n = r._next;
                            if (n is null)
                            {
                                if ((target._last = r._previous) is null)
                                    target._first = null;
                                else
                                {
                                    r._previous = null;
                                    target._last._next = null;
                                }
                            }
                            else
                            {
                                if ((n._previous = r._previous) is null)
                                    target._first = n;
                                else
                                {
                                    n._previous._next = n;
                                    r._previous = null;
                                }
                                r._next = null;
                            }
                            r._target = null;
                            r = n;
                        }
                    }
                }
                return observers;
            }

            internal static IDisposable Subscribe(Observable<TNotice> target, IObserver<TNotice> observer)
            {
                Registration registration;
                lock ((target ?? throw new ArgumentNullException(nameof(target)))._syncRoot)
                {
                    if (target._isDisposed)
                        throw new ObjectDisposedException(target.GetType().FullName);

                    registration = new(target, observer);
                    if ((target._last = registration)._previous is null)
                        target._first = registration;
                    else
                        registration._previous._next = registration;
                }
                return registration;
            }

            private void Dispose(bool disposing)
            {
                Observable<TNotice> target = _target;
                _target = null;
                if (target is null || !disposing)
                    return;
                lock (target._syncRoot)
                {
                    if (_next is null)
                    {
                        if ((target._last = _previous) is null)
                            target._first = null;
                        else
                            _previous = _previous._next = null;
                    }
                    else
                    {
                        if ((_next._previous = _previous) is null)
                            target._first = _next;
                        else
                        {
                            _previous._next = _next;
                            _previous = null;
                        }
                        _next = null;
                    }
                }
            }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            internal static void Dispose(Observable<TNotice> target)
            {
                if (!target._isDisposed)
                    throw new InvalidOperationException();
                Queue<IObserver<TNotice>> observers = new();
                lock (target._syncRoot)
                {
                    Registration p = target._first;
                    if (p is not null)
                    {
                        if (p.Observer.TryGetTarget(out IObserver<TNotice> observer))
                            observers.Enqueue(observer);
                        for (Registration r = p._next; r is not null; r = r._next)
                        {
                            if (r.Observer.TryGetTarget(out observer))
                                observers.Enqueue(observer);
                            p._next = null;
                            p._target = null;
                            (p = r)._previous = null;
                        }
                        target._first = target._last = null;
                    }
                }
                OnDisposed(observers);
            }

            private static void OnDisposed(Queue<IObserver<TNotice>> observers)
            {
                if (observers.TryDequeue(out IObserver<TNotice> observer))
                    try { observer.OnCompleted(); }
                    finally { OnDisposed(observers); }
            }
        }
    }
}
