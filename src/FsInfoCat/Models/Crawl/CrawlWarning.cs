using System;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    public class CrawlWarning : CrawlMessage
    {
        /// <summary>
        /// Create new CrawlWarning.
        /// </summary>
        /// <param name="id">The <seealso cref="MessageId" /> of the warning.</param>
        /// <remarks>The <seealso cref="Message" /> will be obtained using the <seealso cref="DescriptionAttribute" />
        /// of the <seealso cref="MessageId" /> (<paramref name="id" />) value.</remarks>
        public CrawlWarning(MessageId id) : base(id) { }

        /// <summary>
        /// Create new CrawlWarning.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="id">The <seealso cref="MessageId" /> of the warning.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> was null.</exception>
        /// <exception cref="ArgumentException"><paramref name="message" /> was empty or contained only white space.</exception>
        public CrawlWarning(string message, MessageId id) : base(message, id) { }

        public CrawlWarning() { }

        public override string ToString() =>
            $"Warning {ID.ToString("F")} {{ Message=\"{Message}\" }}";
    }
}
