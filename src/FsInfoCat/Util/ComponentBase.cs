using System;
using System.ComponentModel;
using System.Threading;

namespace FsInfoCat.Util
{
    public abstract class ComponentBase : MarshalByRefObject, IComponent
    {
        private object _syncRoot = new object();
        private bool _disposed;
        private ISite _site = null;

        protected event EventHandler Disposed;

        event EventHandler IComponent.Disposed
        {
            add
            {
                if (!_disposed)
                    Disposed += value;
            }
            remove => Disposed -= value;
        }

        ISite IComponent.Site
        {
            get => _site;
            set
            {
                Monitor.Enter(_syncRoot);
                try { _site = value; }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            ISite site;
            Monitor.Enter(_syncRoot);
            try
            {
                if (_disposed)
                    return;
                _disposed = true;
                if (!disposing)
                    return;
                site = _site;
            }
            finally { Monitor.Exit(_syncRoot); }
            try
            {
                if (null != site)
                    site.Container?.Remove(this);
            }
            finally { Disposed?.Invoke(this, EventArgs.Empty); }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
