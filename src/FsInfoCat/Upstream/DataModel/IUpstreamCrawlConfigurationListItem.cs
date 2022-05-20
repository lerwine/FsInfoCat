namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a crawl configuration list item entity.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ICrawlConfigurationListItem" />
    /// <seealso cref="Local.ILocalCrawlConfigurationListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamCrawlConfigurationListItem")]
    public interface IUpstreamCrawlConfigurationListItem : IUpstreamDbEntity, ICrawlConfigurationListItem { }
}
