using FsInfoCat.Util;
using System;

namespace FsInfoCat.Models.Crawl
{
    public abstract class CrawlMessage : ComponentBase
    {
        private string _message = "";
        public string Message
        {
            get => _message;
            set => _message = value.CoerceAsTrimmed();
        }

        public MessageId ID { get; set; }

        protected CrawlMessage(MessageId id)
        {
            ID = id;
            Message = id.GetDescription();
        }

        protected CrawlMessage(string message, MessageId id)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            if ((Message = message.Trim()).Length == 0)
                throw new ArgumentException("Message cannot be empty", nameof(message));
            Message = message;
            ID = id;
        }

        public CrawlMessage() { }
    }
}
