using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsDirectory : ComponentBase, IFsDirectory, IFsChildNode
    {
        public string Name { get; set; }

        private NestedCollectionComponentContainer<FsDirectory, IFsChildNode> _childNodes;
        public Collection<IFsChildNode> ChildNodes
        {
            get => _childNodes.Items;
            set => _childNodes.Items = value;
        }

        private NestedCollectionComponentContainer<FsDirectory, CrawlMessage> _messagesContainer;
        public Collection<CrawlMessage> Messages
        {
            get => _messagesContainer.Items;
            set => _messagesContainer.Items = value;
        }

        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public int Attributes { get; set; }

        public FsDirectory()
        {
            _messagesContainer = new NestedCollectionComponentContainer<FsDirectory, CrawlMessage>(this, false);
            _childNodes = new NestedCollectionComponentContainer<FsDirectory, IFsChildNode>(this, false);
        }

        [Obsolete()]
        public static FsRoot GetRoot(FsHost host, DirectoryInfo directory, out IFsDirectory branch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Looks for the first nested partial crawl.
        /// </summary>
        /// <param name="message">The <seealso cref="PartialCrawlWarning" /> representing the partial crawl.</param>
        /// <param name="segments">The <seealso cref="IFsDirectory" /> representing the path segments, relative to the current directory, for the parent directory of the partial crawl.
        /// The last segment will contain the items that were already crawled.</param>
        /// <returns>True if a <seealso cref="PartialCrawlWarning" /> was found; otherwise, false.</returns>
        public bool TryFindPartialCrawl(out PartialCrawlWarning message, out IEnumerable<IFsDirectory> segments)
        {
            message = Messages.OfType<PartialCrawlWarning>().Where(m => m.NotCrawled.Any(s => !string.IsNullOrWhiteSpace(s))).FirstOrDefault();
            if (null != message)
            {
                segments = new IFsDirectory[0];
                return true;
            }
            foreach (IFsDirectory root in ChildNodes.OfType<IFsDirectory>())
            {
                if (root.TryFindPartialCrawl(out message, out segments))
                {
                    segments = (new IFsDirectory[] { root }).Concat(segments);
                    return true;
                }
            }
            segments = new IFsDirectory[0];
            return false;
        }
    }
}
