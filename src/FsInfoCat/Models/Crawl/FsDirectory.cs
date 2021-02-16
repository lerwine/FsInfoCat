using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsDirectory : ComponentBase, IFsDirectory, IFsChildNode
    {
        private string _name = "";
        private readonly ComponentList.AttachableContainer _container;
        private ComponentList<CrawlMessage> _messagesList;
        private ComponentList<IFsChildNode> _childNodes;

        public string Name
        {
            get => _name;
            set => _name = value ?? "";
        }

        public ComponentList<IFsChildNode> ChildNodes
        {
            get => _childNodes;
            set
            {
                ComponentList<IFsChildNode> list = (value is null) ? new ComponentList<IFsChildNode>() : value;
                if (ReferenceEquals(list, _childNodes))
                    return;
                _container.Detach(_childNodes);
                _container.Attach(list);
                _childNodes = list;
            }
        }
        IList<IFsChildNode> IFsDirectory.ChildNodes { get => _childNodes; set => ChildNodes = (ComponentList<IFsChildNode>)value; }

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

        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public int Attributes { get; set; }

        public FsDirectory()
        {
            _container = new ComponentList.AttachableContainer(this);
            _messagesList = new ComponentList<CrawlMessage>(_container);
            _childNodes = new ComponentList<IFsChildNode>(_container);
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

        public bool Equals(IFsChildNode other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (other is FsDirectory)
                return ((this.GetContainer() is ComponentList.AttachableContainer container) ? container.NameComparer : StringComparer.InvariantCultureIgnoreCase).Equals(Name, other.Name);
            return false;
        }

        public int CompareTo(IFsChildNode other)
        {
            if (other is null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            if (other is FsDirectory)
                return ((this.GetContainer() is ComponentList.AttachableContainer container) ? container.NameComparer : StringComparer.InvariantCultureIgnoreCase).Compare(Name, other.Name);
            return -1;
        }

        public override bool Equals(object obj) => Equals(obj as IFsChildNode);

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;
    }
}
