using System;

namespace FsInfoCat
{
    public abstract class WeakEventRelay<TSource, TEventArgs, THandler>
        where TSource : class
        where TEventArgs : EventArgs
        where THandler : Delegate
    {
        private WeakReference<TSource> _source;
        private readonly WeakReference<THandler> _eventHandler;

        protected WeakEventRelay(THandler eventHandler)
        {
            _eventHandler = new(eventHandler ?? throw new ArgumentNullException(nameof(eventHandler)));
        }

        protected abstract void AttachSource(TSource source);

        protected abstract void DetachSource(TSource source);

        public bool IsAttached()
        {
            lock (_eventHandler)
            {
                if (_source is not null)
                {
                    if (_source.TryGetTarget(out _))
                        return true;
                    _source = null;
                }
            }
            return false;
        }

        public void Attach(TSource source, bool detachCurrent = false)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            lock (_eventHandler)
            {
                if (_source is not null && _source.TryGetTarget(out TSource oldSource))
                {
                    if (ReferenceEquals(source, oldSource))
                        return;
                    if (detachCurrent)
                        DetachSource(oldSource);
                    else
                        throw new InvalidOperationException();
                }
                AttachSource(source);
                _source = new(source);
            }
        }
        public void Detach()
        {
            lock (_eventHandler)
            {
                if (_source is null)
                    return;
                if (_source.TryGetTarget(out TSource sender))
                    DetachSource(sender);
                _source = null;
            }
        }

        protected void RaiseEvent(object sender, TEventArgs e)
        {
            if (_eventHandler.TryGetTarget(out THandler eventHandler))
                _ = eventHandler.DynamicInvoke(sender, e);
            else
                Detach();
        }
    }

    public abstract class WeakEventRelay<TSource, TEventArgs> : WeakEventRelay<TSource, TEventArgs, EventHandler<TEventArgs>>
        where TSource : class
        where TEventArgs : EventArgs
    {
        public WeakEventRelay(EventHandler<TEventArgs> eventHandler) : base(eventHandler) { }
    }
}
