using System;
using System.ComponentModel;
using System.Threading;

namespace FsInfoCat.Models.Crawl
{
    public abstract partial class CrawlComponent
    {
        public abstract class NestedCrawlComponentContainer<TOwner, TComponent> : CrawlComponentContainer<TComponent>, INestedContainer
            where TOwner : CrawlComponent
            where TComponent : CrawlComponent<TOwner, TComponent>
        {
            public CrawlComponent Owner { get; }
            IComponent INestedContainer.Owner => Owner;

            public NestedCrawlComponentContainer(TOwner owner)
            {
                if (null == owner)
                    throw new ArgumentNullException(nameof(owner));

                Monitor.Enter(owner.SyncRoot);
                try
                {
                    if (owner._disposed)
                        throw new ObjectDisposedException(owner.GetType().FullName);
                    Owner = owner;
                    owner.Disposed += OnOwnerDisposed;
                }
                finally { Monitor.Exit(owner.SyncRoot); }
            }

            private void OnOwnerDisposed(object sender, EventArgs e)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Monitor.Enter(Owner.SyncRoot);
                    try
                    {
                        if (!Owner._disposed)
                            return;
                        Owner.Disposed -= OnOwnerDisposed;
                        Dispose();
                    }
                    finally { Monitor.Exit(Owner.SyncRoot); }
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            protected override CrawlComponentSite CreateSite(CrawlComponentContainer container, CrawlComponent component) => new NestedCrawlComponentSite<TOwner, TComponent>((NestedCrawlComponentContainer<TOwner, TComponent>)container, (TComponent)component);

            public override object GetService(Type serviceType)
            {
                return (serviceType == typeof(INestedContainer)) ? this : base.GetService(serviceType);
            }

            protected override void Dispose(bool disposing)
            {
                try
                {
                    if (!disposing)
                        return;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        Monitor.Enter(Owner.SyncRoot);
                        try
                        {
                            if (!Owner._disposed)
                            {
                                Owner.Disposed -= OnOwnerDisposed;
                                Owner.Dispose();
                            }
                        }
                        finally { Monitor.Exit(Owner.SyncRoot); }
                    }
                    finally
                    {
                        Monitor.Exit(SyncRoot);
                        base.Dispose(disposing);
                    }
                }
                finally { base.Dispose(disposing); }
            }
        }
    }
}
