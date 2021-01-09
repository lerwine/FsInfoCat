using System;
using System.Collections.ObjectModel;
using System.IO;

namespace FsInfoCat.Models.Crawl
{
    public class FsFile : IFsChildNode
    {
        public string Name { get; set; }

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

        public long Length { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public int Attributes { get; set; }
    }
}
