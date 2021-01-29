using System;
using System.Collections.ObjectModel;
using System.IO;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    public class FsFile : ComponentBase, IFsChildNode
    {
        public string Name { get; set; }

        private NestedCollectionComponentContainer<FsFile, CrawlMessage> _messagesContainer;
        public Collection<CrawlMessage> Messages
        {
            get => _messagesContainer.Items;
            set => _messagesContainer.Items = value;
        }

        public long Length { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public int Attributes { get; set; }

        public FsFile()
        {
            _messagesContainer = new NestedCollectionComponentContainer<FsFile, CrawlMessage>(this, false);
        }
    }
}
