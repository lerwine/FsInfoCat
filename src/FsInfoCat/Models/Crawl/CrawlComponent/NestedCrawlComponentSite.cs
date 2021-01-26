using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl
{
    public abstract partial class CrawlComponent
    {
        public class NestedCrawlComponentSite<TOwner, TComponent> : CrawlComponentSite<TComponent>, INestedSite
            where TOwner : CrawlComponent
            where TComponent : CrawlComponent<TOwner, TComponent>
        {
            public string FullName => throw new NotImplementedException();

            public NestedCrawlComponentSite(NestedCrawlComponentContainer<TOwner, TComponent> container, TComponent component) : base(container, component) { }

            public override object GetService(Type serviceType)
            {
                return (serviceType == typeof(INestedSite)) ? this : base.GetService(serviceType);
            }
        }
    }
}
