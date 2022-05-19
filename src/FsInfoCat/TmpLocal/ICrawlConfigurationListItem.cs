using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a crawl configuration list item entity.
    /// </summary>
    /// <seealso cref="M.ICrawlConfigurationListItem" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ILocalCrawlConfigReportItem" />
    /// <seealso cref="ILocalDbContext.CrawlConfigListing" />
    /// <seealso cref="Upstream.Model.ICrawlConfigurationListItem" />
    public interface ILocalCrawlConfigurationListItem : ILocalCrawlConfigurationRow, M.ICrawlConfigurationListItem { }
}
