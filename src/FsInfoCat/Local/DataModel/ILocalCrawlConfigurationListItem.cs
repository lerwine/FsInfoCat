using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a crawl configuration list item entity.
    /// </summary>
    /// <seealso cref="ICrawlConfigurationListItem" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ILocalCrawlConfigReportItem" />
    /// <seealso cref="ILocalDbContext.CrawlConfigListing" />
    /// <seealso cref="Upstream.IUpstreamCrawlConfigurationListItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalCrawlConfigurationListItem")]
    public interface ILocalCrawlConfigurationListItem : ILocalCrawlConfigurationRow, ICrawlConfigurationListItem { }
}
