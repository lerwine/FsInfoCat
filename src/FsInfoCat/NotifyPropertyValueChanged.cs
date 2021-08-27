using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FsInfoCat
{
    public abstract class NotifyPropertyValueChanged : INotifyPropertyValueChanged, IObservable<PropertyValueChangedEventArgs>
    {
        private readonly ILogger<NotifyPropertyValueChanged> _logger = Services.ServiceProvider.GetRequiredService<ILogger<NotifyPropertyValueChanged>>();
        private Subscription _firstSubscribed;
        private Subscription _lastSubscribed;

        public event PropertyValueChangedEventHandler PropertyValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        protected object SyncRoot { get; } = new();

        protected virtual bool CheckHashSetChanged<T>(HashSet<T> oldValue, HashSet<T> newValue, Action<HashSet<T>> setter, [CallerMemberName] string propertyName = null)
        {
            if (newValue is null)
            {
                if (oldValue.Count == 0)
                    return false;
                setter(new HashSet<T>());
            }
            else
            {
                if (ReferenceEquals(oldValue, newValue))
                    return false;
                setter(newValue);
            }
            RaisePropertyChanged(oldValue, newValue, propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged(PropertyValueChangedEventArgs args)
        {
            try { Subscription.OnPropertyChanged(this, args); }
            finally
            {
                try { PropertyValueChanged?.Invoke(this, args); }
                finally { PropertyChanged?.Invoke(this, args); }
            }
            try { PropertyValueChanged?.Invoke(this, args); }
            finally { PropertyChanged?.Invoke(this, args); }
        }

        protected void RaisePropertyChanged<T>(T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            using var scope = _logger.BeginScope("Property {propertyName} changed", propertyName);
            OnPropertyChanged(new PropertyValueChangedEventArgs(propertyName, oldValue, newValue));
        }

        public IDisposable Subscribe([DisallowNull] IObserver<PropertyValueChangedEventArgs> observer) => new Subscription(this, observer);

        class Subscription : IDisposable
        {
            private Subscription _previous;
            private Subscription _next;
            private readonly NotifyPropertyValueChanged _owner;

            internal WeakReference<IObserver<PropertyValueChangedEventArgs>> Observer { get; private set; }

            public Subscription([DisallowNull] NotifyPropertyValueChanged owner, [DisallowNull] IObserver<PropertyValueChangedEventArgs> observer)
            {
                lock (_owner.SyncRoot)
                {
                    if ((_previous = (_owner = owner)._lastSubscribed) is null)
                        _owner._firstSubscribed = _owner._lastSubscribed = this;
                    else
                        _previous._next = _owner._lastSubscribed = this;
                    Observer = new WeakReference<IObserver<PropertyValueChangedEventArgs>>(observer ?? throw new ArgumentNullException(nameof(observer)));
                }
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                    lock (_owner.SyncRoot)
                    {
                        if (Observer is null)
                            return;
                        Observer = null;
                        if (_next is null)
                        {
                            if ((_owner._lastSubscribed = _previous) is null)
                                _owner._firstSubscribed = null;
                            else
                                _previous = _previous._next = null;
                        }
                        else if ((_next._previous = _previous) is null)
                            _next = (_owner._firstSubscribed = _next)._previous = null;
                        else
                        {
                            _previous._next = _next;
                            _previous = _next = null;
                        }
                    }
                else
                    Observer = null;
            }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            internal static void OnPropertyChanged(NotifyPropertyValueChanged sender, PropertyValueChangedEventArgs args)
            {
                // TODO: Implement push notification
                throw new NotImplementedException();
            }
        }
    }
}
