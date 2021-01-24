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

        /// <summary>
        /// Create new CrawlWarning.
        /// </summary>
        /// <param name="id">The <seealso cref="MessageId" /> of the warning.</param>
        /// <remarks>The <seealso cref="Message" /> will be obtained using the <seealso cref="DescriptionAttribute" />
        /// of the <seealso cref="MessageId" /> (<paramref name="id" />) value.</remarks>
        public CrawlWarning(MessageId id)
        {
            ID = id;
            Type t = id.GetType();
            string name = Enum.GetName(t, id);
            Message = t.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>().Select(a => a.Description)
                .Where(d => !string.IsNullOrWhiteSpace(d)).DefaultIfEmpty(name).First();
        }

        /// <summary>
        /// Create new CrawlWarning.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="id">The <seealso cref="MessageId" /> of the warning.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> was null.</exception>
        /// <exception cref="ArgumentException"><paramref name="message" /> was empty or contained only white space.</exception>
        public CrawlWarning(string message, MessageId id)
        {
            if (null == message)
                throw new ArgumentNullException(nameof(message));
            if ((Message = message.Trim()).Length == 0)
                throw new ArgumentException("Message cannot be empty", nameof(message));
            Message = message;
            ID = id;
        }

        public CrawlWarning() { }
    }
}
