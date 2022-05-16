namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a crawl configuration entity.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ICrawlConfigurationRow" />
    /// <seealso cref="Local.ILocalCrawlConfigurationRow" />
    public interface IUpstreamCrawlConfigurationRow : IUpstreamDbEntity, ICrawlConfigurationRow { }
}
