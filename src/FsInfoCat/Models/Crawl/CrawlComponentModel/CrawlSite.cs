using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public partial class CrawlComponent
    {
        public partial class CrawlSite : ICrawlSite
        {
            public string Name => throw new NotImplementedException();

            public CrawlComponent Component => throw new NotImplementedException();

            public CrawlComponentContainer Container => throw new NotImplementedException();

            public bool Equals(ICrawlSite other)
            {
                throw new NotImplementedException();
            }

            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }

            ICrawlComponent ICrawlSite.Component => throw new NotImplementedException();
            IComponent ISite.Component => throw new NotImplementedException();
            ICrawlComponentContainer ICrawlSite.Container => throw new NotImplementedException();
            IContainer ISite.Container => throw new NotImplementedException();
            bool ISite.DesignMode => throw new NotImplementedException();
            string ISite.Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        }
    }
}
