using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a crawl configuration list item entity.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ICrawlConfigurationListItem" />
    /// <seealso cref="Local.Model.ILocalCrawlConfigurationListItem" />
    public interface IUpstreamCrawlConfigurationListItem : IUpstreamDbEntity, ICrawlConfigurationListItem { }
}
