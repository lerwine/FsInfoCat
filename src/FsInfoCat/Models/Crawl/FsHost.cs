using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    /// <summary>
    /// Represents a file system host machine.
    /// </summary>
    public sealed partial class FsHost : ComponentBase, IFsNode
    {
        private NestedCollectionComponentContainer<FsHost, FsRoot> _roots;

        /// <summary>
        /// File system roots containing roots crawled.
        /// </summary>
        public Collection<FsRoot> Roots
        {
            get => _roots.Items;
            set => _roots.Items = value;
        }

        private NestedCollectionComponentContainer<FsHost, CrawlMessage> _messagesContainer;
        public Collection<CrawlMessage> Messages
        {
            get => _messagesContainer.Items;
            set => _messagesContainer.Items = value;
        }

        public string MachineName { get; set; }

        public string MachineIdentifier { get; set; }

        string INamedComponent.Name => (string.IsNullOrWhiteSpace(MachineName)) ? MachineIdentifier ?? "" : MachineName;

        public FsHost()
        {
            _messagesContainer = new NestedCollectionComponentContainer<FsHost, CrawlMessage>(this, false);
            _roots = new NestedCollectionComponentContainer<FsHost, FsRoot>(this, false);
        }

        /// <summary>
        /// Looks for the first nested partial crawl.
        /// </summary>
        /// <param name="message">The <seealso cref="PartialCrawlWarning" /> representing the partial crawl.</param>
        /// <param name="segments">The <seealso cref="IFsDirectory" /> representing the path segments of the parent directory of the partial crawl.
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
            foreach (FsRoot root in Roots)
            {
                if (null != root && root.TryFindPartialCrawl(out message, out segments))
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
