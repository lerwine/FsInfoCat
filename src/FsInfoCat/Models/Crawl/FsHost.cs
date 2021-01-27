using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    /// <summary>
    /// Represents a file system host machine.
    /// </summary>
    public sealed partial class FsHost : IFsNode
    {
        private Collection<FsRoot> _roots = null;

        /// <summary>
        /// File system roots containing roots crawled.
        /// </summary>
        public Collection<FsRoot> Roots
        {
            get
            {
                Collection<FsRoot> roots = _roots;
                if (null == roots)
                    _roots = roots = new Collection<FsRoot>();
                return roots;
            }
            set { _roots = value; }
        }

        private Collection<ICrawlMessage> _messages = null;

        public Collection<ICrawlMessage> Messages
        {
            get
            {
                Collection<ICrawlMessage> messages = _messages;
                if (null == messages)
                    _messages = messages = new Collection<ICrawlMessage>();
                return messages;
            }
            set { _messages = value; }
        }

        public string MachineName { get; set; }

        public string MachineIdentifier { get; set; }

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
