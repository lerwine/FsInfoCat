using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl
{
    public abstract partial class CrawlComponent : IComponent
    {
        private bool _disposed = false;
        private CrawlComponentSite _site = null;

        protected object SyncRoot { get; } = new object();

        public CrawlComponentSite Site => _site;

        ISite IComponent.Site { get => Site; set => throw new NotSupportedException(); }

        public event EventHandler Disposed;

        protected abstract string GetName();

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            try
            {
                if (disposing)
                    Disposed.Invoke(this, EventArgs.Empty);
            }
            finally { _disposed = true; }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public abstract class CrawlComponent<TOwner, TComponent> : CrawlComponent
        where TOwner : CrawlComponent
        where TComponent : CrawlComponent<TOwner, TComponent>
    {
        public new NestedCrawlComponentSite<TOwner, TComponent> Site => (NestedCrawlComponentSite<TOwner, TComponent>)base.Site;
    }
}
