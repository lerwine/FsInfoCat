using System.Collections.ObjectModel;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsHost : IFsNode
    {
        private Collection<FsRoot> _roots = null;
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
    }
}
