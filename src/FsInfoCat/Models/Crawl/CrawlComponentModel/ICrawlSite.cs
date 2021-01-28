using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public interface ICrawlSite : ISite, IEquatable<ICrawlSite>
    {
        new string Name { get; }
        new ICrawlComponent Component { get; }
        new ICrawlComponentContainer Container { get; }
    }

    public partial class CrawlComponent
    {
        public abstract partial class CrawlSite : ICrawlSite
        {
            public string Name => Component.GetName();

            public CrawlComponent Component { get; }

            IComponent ISite.Component => Component;

            ICrawlComponent ICrawlSite.Component => Component;

            public ICrawlComponentContainer Container { get; }

            ICrawlComponentContainer ICrawlSite.Container => Container;

            IContainer ISite.Container => Container;

            string ISite.Name { get => Component.GetName(); set => throw new NotSupportedException(); }

            bool ISite.DesignMode => false;

            public CrawlSite(ICrawlComponentContainer container, CrawlComponent component)
            {
                if (null == container)
                    throw new ArgumentNullException(nameof(container));
                if (null == component)
                    throw new ArgumentNullException(nameof(component));
                Container = container;
                Component = component;
            }

            public bool Equals(ICrawlSite other)
            {
                throw new NotImplementedException();
            }

            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }
        }
    }

    public abstract class CrawlSite<TContainer> : CrawlComponent.CrawlSite
        where TContainer : ICrawlComponentContainer
    {
        public new TContainer Container => (TContainer)base.Container;

        public CrawlSite(TContainer container, CrawlComponent component) : base(container, component) { }
    }

    public class CrawlSite<TContainer, TComponent> : CrawlSite<TContainer>
        where TContainer : ICrawlComponentContainer
        where TComponent : CrawlComponent
    {
        public new TComponent Component => (TComponent)base.Component;

        public CrawlSite(TContainer container, TComponent component) : base(container, component) { }
    }
}
