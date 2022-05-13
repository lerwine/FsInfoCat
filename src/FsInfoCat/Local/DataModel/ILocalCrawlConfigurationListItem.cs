using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a crawl configuration list item entity.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ICrawlConfigurationListItem" />
    public interface ILocalCrawlConfigurationListItem : ILocalDbEntity, ICrawlConfigurationListItem { }
}
