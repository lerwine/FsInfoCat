namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a crawl configuration entity.
    /// </summary>
    /// <seealso cref="ICrawlConfigurationRow" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="Upstream.IUpstreamCrawlConfigurationRow" />
    public interface ILocalCrawlConfigurationRow : ILocalDbEntity, ICrawlConfigurationRow { }
}
