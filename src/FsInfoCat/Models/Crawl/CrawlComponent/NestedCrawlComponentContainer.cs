using System;
using System.ComponentModel;

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

                Owner = owner;
            }

            protected override CrawlComponentSite CreateSite(CrawlComponentContainer container, CrawlComponent component) => new NestedCrawlComponentSite<TOwner, TComponent>((NestedCrawlComponentContainer<TOwner, TComponent>)container, (TComponent)component);

            public override object GetService(Type serviceType)
            {
                return (serviceType == typeof(INestedContainer)) ? this : base.GetService(serviceType);
            }
        }
    }
}
