namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a crawl configuration entity.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ICrawlConfigurationRow" />
    /// <seealso cref="Local.ILocalCrawlConfigurationRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamCrawlConfigurationRow")]
    public interface IUpstreamCrawlConfigurationRow : IUpstreamDbEntity, ICrawlConfigurationRow { }
}
