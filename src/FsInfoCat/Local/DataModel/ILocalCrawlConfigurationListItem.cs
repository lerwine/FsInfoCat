using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a crawl configuration list item entity.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ICrawlConfigurationListItem" />
    /// <seealso cref="Upstream.IUpstreamCrawlConfigurationListItem" />
    public interface ILocalCrawlConfigurationListItem : ILocalDbEntity, ICrawlConfigurationListItem { }
}
