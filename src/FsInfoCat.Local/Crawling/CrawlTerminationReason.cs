using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    /// <summary>
    /// Describes the reason why a file system crawl was terminated.
    /// </summary>
    public enum CrawlTerminationReason
    {
        /// <summary>
        /// Crawl was completed successfully.
        /// </summary>
        Completed,

        /// <summary>
        /// Crawl was stopped because <see cref="ICrawlSettings.MaxTotalItems"/> was processed.
        /// </summary>
        ItemLimitReached,

        /// <summary>
        /// Crawl stopped because the <see cref="ICrawlSettings.TTL"/> limit was reached.
        /// </summary>
        TimeLimitReached,

        /// <summary>
        /// Crawl was aborted because the crawl <see cref="TaskStatus.Faulted"/> or was <see cref="TaskStatus.Canceled"/>.
        /// </summary>
        Aborted
    }
}
