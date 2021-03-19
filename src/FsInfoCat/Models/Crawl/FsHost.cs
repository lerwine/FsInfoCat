using FsInfoCat.Util;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    /// <summary>
    /// Represents a file system host machine.
    /// </summary>
    public sealed partial class FsHost : ComponentBase, IFsNode
    {
        private string _machineName = "";
        private string _machineIdentifier = "";
        private readonly ComponentList.AttachableContainer _container;
        private ComponentList<CrawlMessage> _messagesList;
        private ComponentList<FsRoot> _rootsList;


        /// <summary>
        /// File system roots containing roots crawled.
        /// </summary>
        public ComponentList<FsRoot> Roots
        {
            get => _rootsList;
            set
            {
                ComponentList<FsRoot> list = (value is null) ? new ComponentList<FsRoot>() : value;
                if (ReferenceEquals(list, _rootsList))
                    return;
                _container.Detach(_rootsList);
                _container.Attach(list);
                _rootsList = list;
            }
        }

        public ComponentList<CrawlMessage> Messages
        {
            get => _messagesList;
            set
            {
                ComponentList<CrawlMessage> list = (value is null) ? new ComponentList<CrawlMessage>() : value;
                if (ReferenceEquals(list, _messagesList))
                    return;
                _container.Detach(_messagesList);
                _container.Attach(list);
                _messagesList = list;
            }
        }

        IList<CrawlMessage> IFsNode.Messages { get => Messages; set => Messages = (ComponentList<CrawlMessage>)value; }

        public string MachineName
        {
            get => _machineName;
            set => _machineName = value ?? "";
        }

        public string MachineIdentifier
        {
            get => _machineIdentifier;
            set => _machineIdentifier = value ?? "";
        }

        string INamedComponent.Name => (string.IsNullOrWhiteSpace(MachineName)) ? MachineIdentifier ?? "" : MachineName;

        public FsHost()
        {
            _container = new ComponentList.AttachableContainer(this);
            _messagesList = new ComponentList<CrawlMessage>(_container);
            _rootsList = new ComponentList<FsRoot>(_container);
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
                segments = System.Array.Empty<IFsDirectory>();
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
            segments = System.Array.Empty<IFsDirectory>();
            return false;
        }
    }
}
