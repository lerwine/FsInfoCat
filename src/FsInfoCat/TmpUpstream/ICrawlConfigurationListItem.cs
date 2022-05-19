using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a crawl configuration list item entity.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="M.ICrawlConfigurationListItem" />
    /// <seealso cref="Local.Model.ICrawlConfigurationListItem" />
    public interface IUpstreamCrawlConfigurationListItem : IUpstreamDbEntity, M.ICrawlConfigurationListItem { }
}
