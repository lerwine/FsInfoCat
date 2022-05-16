namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a crawl configuration list item entity.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ICrawlConfigurationListItem" />
    /// <seealso cref="Local.ILocalCrawlConfigurationListItem" />
    public interface IUpstreamCrawlConfigurationListItem : IUpstreamDbEntity, ICrawlConfigurationListItem { }
}
