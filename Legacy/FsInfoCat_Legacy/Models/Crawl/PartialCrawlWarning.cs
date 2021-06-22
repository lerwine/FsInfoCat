using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    /// <summary>
    /// Warning message indicating that the crawl job was stopped before the crawl was completed.
    /// </summary>
    public class PartialCrawlWarning : CrawlWarning
    {
        private const string ERROR_MESSAGE_NO_UNCRAWLED_ITEMS = "No uncrawled item names provided";

        private Collection<string> _notCrawled = null;

        /// <summary>
        /// Names of items not crawled.
        /// </summary>
        /// <value>For partial subdirectory crawls, this will be the name of the files and folder. For partial root crawls, this will be the paths not crawled.</value>
        public Collection<string> NotCrawled
        {
            get
            {
                Collection<string> roots = _notCrawled;
                if (roots is null)
                    _notCrawled = roots = new Collection<string>();
                return roots;
            }
            set { _notCrawled = value; }
        }

        /// <summary>
        /// Create new PartialCrawlWarning.
        /// </summary>
        /// <param name="id">The <seealso cref="MessageId" /> of the warning.</param>
        /// <param name="notCrawled">The names of items not crawled.</param>
        /// <exception cref="ArgumentNullException"><paramref name="notCrawled" /> was null.</exception>
        /// <exception cref="ArgumentException"><paramref name="notCrawled" /> was empty or or all items contained only white space.</exception>
        /// <remarks>The <seealso cref="Message" /> will be obtained using the <seealso cref="DescriptionAttribute" />
        /// of the <seealso cref="MessageId" /> (<paramref name="id" />) value.</remarks>
        public PartialCrawlWarning(MessageId id, IEnumerable<string> notCrawled) : base(id)
        {
            if (notCrawled is null)
                throw new ArgumentNullException(nameof(notCrawled));
            _notCrawled = new Collection<string>(notCrawled.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());
            if (_notCrawled.Count == 0)
                throw new ArgumentException(ERROR_MESSAGE_NO_UNCRAWLED_ITEMS, nameof(notCrawled));
        }

        /// <summary>
        /// Create new PartialCrawlWarning.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="id">The <seealso cref="MessageId" /> of the warning.</param>
        /// <param name="notCrawled">The names of items not crawled.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> or <paramref name="message" /> was null.</exception>
        /// <exception cref="ArgumentException"><paramref name="message" /> was empty or contained only white space.
        /// <para>- or -</para>
        /// <para><paramref name="notCrawled" /> was empty or or all items contained only white space.</para></exception>
        public PartialCrawlWarning(string message, MessageId id, IEnumerable<string> notCrawled) : base(id)
        {
            if (notCrawled is null)
                throw new ArgumentNullException(nameof(notCrawled));
            _notCrawled = new Collection<string>(notCrawled.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());
            if (_notCrawled.Count == 0)
                throw new ArgumentException(ERROR_MESSAGE_NO_UNCRAWLED_ITEMS, nameof(notCrawled));
        }

        public PartialCrawlWarning() : base() { }
    }
}
