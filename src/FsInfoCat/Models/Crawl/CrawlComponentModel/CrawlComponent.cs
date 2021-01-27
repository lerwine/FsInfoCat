using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public abstract partial class CrawlComponent : ICrawlComponent
    {
        private bool disposedValue;

        protected CrawlSite Site => _site;

        public event EventHandler Disposed;

        protected abstract string GetName();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CrawlComponent()
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

        ICrawlSite ICrawlComponent.Site => throw new NotImplementedException();
        ISite IComponent.Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string ICrawlComponent.GetName()
        {
            throw new NotImplementedException();
        }
    }
}
