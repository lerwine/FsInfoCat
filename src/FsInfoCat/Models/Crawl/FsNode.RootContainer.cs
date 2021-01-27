using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    public partial class FsHost
    {
        public class RootContainer : CrawlComponent.NestedCrawlComponentContainer<FsHost, FsRoot>
        {
            internal RootContainer(FsHost owner) : base(owner)
            {
                if (null == owner)
                    throw new ArgumentNullException();
                if (null == owner._roots)
                    throw new InvalidOperationException();
            }

            public override StringComparer GetNameComparer()
            {
                throw new NotImplementedException();
            }
        }
    }
}
