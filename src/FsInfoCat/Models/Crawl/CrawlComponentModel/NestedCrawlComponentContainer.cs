using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public class NestedCrawlComponentContainer : CrawlComponent.CrawlComponentContainer, INestedCrawlComponentContainer
    {
        public CrawlComponent Owner => throw new NotImplementedException();

        protected ComponentCollection Components => throw new NotImplementedException();

        IComponent INestedContainer.Owner => throw new NotImplementedException();

        ICrawlComponent INestedCrawlComponentContainer.Owner => throw new NotImplementedException();

        ComponentCollection IContainer.Components => throw new NotImplementedException();

        protected void Add(NestedCrawlComponent component)
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        protected void Remove(NestedCrawlComponent component)
        {
            throw new NotImplementedException();
        }

        void INestedCrawlComponentContainer.Add(INestedCrawlComponent component)
        {
            throw new NotImplementedException();
        }

        void INestedCrawlComponentContainer.Add(INestedCrawlComponent component, string name)
        {
            throw new NotImplementedException();
        }

        void INestedCrawlComponentContainer.Remove(INestedCrawlComponent component)
        {
            throw new NotImplementedException();
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class NestedCrawlComponentContainer<TOwner> : NestedCrawlComponentContainer, INestedCrawlComponentContainer<TOwner>
        where TOwner : ICrawlComponent
    {
        public new TOwner Owner => throw new NotImplementedException();

        protected void Add(NestedCrawlComponent<TOwner> component)
        {
            throw new NotImplementedException();
        }

        protected void Remove(NestedCrawlComponent<TOwner> component)
        {
            throw new NotImplementedException();
        }

        void INestedCrawlComponentContainer<TOwner>.Add(INestedCrawlComponent<TOwner> component)
        {
            throw new NotImplementedException();
        }

        void INestedCrawlComponentContainer<TOwner>.Add(INestedCrawlComponent<TOwner> component, string name)
        {
            throw new NotImplementedException();
        }

        void INestedCrawlComponentContainer<TOwner>.Remove(INestedCrawlComponent<TOwner> component)
        {
            throw new NotImplementedException();
        }
    }
}
