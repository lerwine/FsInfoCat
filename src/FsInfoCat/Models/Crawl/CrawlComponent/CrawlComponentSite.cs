using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl
{
    public abstract partial class CrawlComponent
    {
        public class CrawlComponentSite : ISite
        {
            public CrawlComponent Component { get; }

            public CrawlComponentContainer Container { get; }

            public string Name => Component.GetName();

            public CrawlComponentSite(CrawlComponentContainer container, CrawlComponent component)
            {
                if (null == container)
                    throw new ArgumentNullException(nameof(container));

                if (null == component)
                    throw new ArgumentNullException(nameof(component));

                Container = container;
                Component = component;
            }

            IComponent ISite.Component => Component;

            IContainer ISite.Container => Container;

            bool ISite.DesignMode => false;

            string ISite.Name { get => Component.GetName(); set => throw new NotSupportedException(); }

            public virtual object GetService(Type serviceType)
            {
                return (serviceType == typeof(ISite)) ? this : Container.GetService(serviceType);
            }
        }

        public class CrawlComponentSite<TComponent> : CrawlComponentSite
            where TComponent : CrawlComponent
        {
            public new TComponent Component => (TComponent)base.Component;

            public new CrawlComponentContainer<TComponent> Container => (CrawlComponentContainer<TComponent>)base.Container;

            public CrawlComponentSite(CrawlComponentContainer<TComponent> container, TComponent component) : base(container, component) { }
        }
    }
}
