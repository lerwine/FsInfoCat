using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    /// <summary>
    /// Named crawl component
    /// </summary>
    public interface ICrawlComponent : IComponent
    {
        /// <summary>
        /// Gets the name to associate with the component.
        /// </summary>
        /// <returns>The name to associate with the component or null if it has no name.</returns>
        string GetName();

        /// <summary>
        /// The site object.
        /// </summary>
        new ICrawlSite Site { get; }
    }

    public abstract partial class CrawlComponent : ICrawlComponent
    {
        private bool _isDisposed = false;

        protected internal object SyncRoot { get; } = new object();

        protected internal CrawlSite Site { get; protected set; }

        ICrawlSite ICrawlComponent.Site => Site;

        ISite IComponent.Site { get => Site; set => throw new NotSupportedException(); }

        protected internal event EventHandler Disposed;

        event EventHandler IComponent.Disposed { add => Disposed += value; remove => Disposed -= value; }

        protected abstract string GetName();

        string ICrawlComponent.GetName() => GetName();

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
