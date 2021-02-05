using System;
using System.ComponentModel;
using System.Linq;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    public abstract class CrawlMessage : ComponentBase
    {
        private string _message = "";
        public string Message
        {
            get => _message;
            set => _message = ModelHelper.CoerceAsTrimmed(value);
        }

        public MessageId ID { get; set; }

        protected CrawlMessage(MessageId id)
        {
            ID = id;
            Type t = id.GetType();
            string name = Enum.GetName(t, id);
            Message = t.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>().Select(a => a.Description)
                .Where(d => !string.IsNullOrWhiteSpace(d)).DefaultIfEmpty(name).First();
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
