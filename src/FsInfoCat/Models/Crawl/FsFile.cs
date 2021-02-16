using System;
using System.Collections.Generic;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsFile : ComponentBase, IFsChildNode
    {
        private string _name = "";
        private readonly ComponentList.AttachableContainer _container;
        private ComponentList<CrawlMessage> _messagesList;

        public string Name
        {
            get => _name;
            set => _name = value ?? "";
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

        public long Length { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public int Attributes { get; set; }

        public FsFile()
        {
            _container = new ComponentList.AttachableContainer(this);
            _messagesList = new ComponentList<CrawlMessage>(_container);
        }

        public bool Equals(IFsChildNode other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (other is FsFile)
                return ((this.GetContainer() is ComponentList.AttachableContainer container) ? container.NameComparer : StringComparer.InvariantCultureIgnoreCase).Equals(Name, other.Name);
            return false;
        }

        public int CompareTo(IFsChildNode other)
        {
            if (other is null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            if (other is FsFile)
                return ((this.GetContainer() is ComponentList.AttachableContainer container) ? container.NameComparer : StringComparer.InvariantCultureIgnoreCase).Compare(Name, other.Name);
            return 1;
        }

        public override bool Equals(object obj) => Equals(obj as IFsChildNode);

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;
    }
}
