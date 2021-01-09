using System;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    public class CrawlWarning : ICrawlMessage
    {
        private string _message = "";

        public string Message
        {
            get => _message;
            set => _message = ModelHelper.CoerceAsTrimmed(value);
        }

        public MessageId ID { get; set; }

        public CrawlWarning(MessageId id)
        {
            ID = id;
            Type t = id.GetType();
            string name = Enum.GetName(t, id);
            Message = t.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>().Select(a => a.Description)
                .Where(d => !string.IsNullOrWhiteSpace(d)).DefaultIfEmpty(name).First();
        }

        public CrawlWarning(string message, MessageId id)
        {
            if (null == message)
                throw new ArgumentNullException(nameof(message));
            if ((Message = message).Length == 0)
                throw new ArgumentException("Message cannot be empty", nameof(message));
            Message = message;
            ID = id;
        }

        public CrawlWarning() { }
    }
}
