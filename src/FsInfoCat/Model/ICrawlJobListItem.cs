using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Represents a file system crawl job log list item entity.
    /// </summary>
    /// <seealso cref="IDbContext.CrawlJobListing" />
    public interface ICrawlJobListItem : ICrawlJobLogRow, IEquatable<ICrawlJobListItem>
    {
        /// <summary>
        /// Gets the primary key of the crawl configuration settings entity.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id"/> of the <see cref="ICrawlConfigurationRow">crawl configuration</see> used for the crawl job.</value>
        Guid ConfigurationId { get; }

        /// <summary>
        /// Gets the primary key of the crawl configuration settings entity.
        /// </summary>
        /// <value>The <see cref="ICrawlConfigurationRow.DisplayName"/> of the <see cref="ICrawlConfigurationRow">crawl configuration</see> used for the crawl job.</value>
        string ConfigurationDisplayName { get; }
    }
}
