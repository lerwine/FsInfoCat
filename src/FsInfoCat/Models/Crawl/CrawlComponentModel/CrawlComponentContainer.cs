using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public partial class CrawlComponent
    {
        private CrawlSite _site;
        public partial class CrawlSite
        {
            private CrawlSite _previous;
            private CrawlSite _next;
            public abstract partial class CrawlComponentContainer : ICrawlComponentContainer
            {
                private CrawlSite _first;
                private CrawlSite _last;
                private bool disposedValue;

                protected ComponentCollection Components => throw new NotImplementedException();

                ComponentCollection IContainer.Components => throw new NotImplementedException();

                protected void Add(ICrawlComponent component)
                {
                    throw new NotImplementedException();
                }

                protected void Add(ICrawlComponent component, string name)
                {
                    throw new NotImplementedException();
                }

                protected void Add(IComponent component)
                {
                    throw new NotImplementedException();
                }

                protected void Add(IComponent component, string name)
                {
                    throw new NotImplementedException();
                }

                public object GetService(Type serviceType)
                {
                    throw new NotImplementedException();
                }

                protected void Remove(ICrawlComponent component)
                {
                    throw new NotImplementedException();
                }

                protected void Remove(IComponent component)
                {
                    throw new NotImplementedException();
                }

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
                // ~CrawlComponentContainer()
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

                void ICrawlComponentContainer.Add(ICrawlComponent component)
                {
                    throw new NotImplementedException();
                }

                void ICrawlComponentContainer.Add(ICrawlComponent component, string name)
                {
                    throw new NotImplementedException();
                }

                void ICrawlComponentContainer.Remove(ICrawlComponent component)
                {
                    throw new NotImplementedException();
                }

                void IContainer.Add(IComponent component)
                {
                    throw new NotImplementedException();
                }

                void IContainer.Add(IComponent component, string name)
                {
                    throw new NotImplementedException();
                }

                void IContainer.Remove(IComponent component)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
