using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a crawl configuration entity.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="M.ICrawlConfigurationRow" />
    /// <seealso cref="Local.Model.ICrawlConfigurationRow" />
    public interface IUpstreamCrawlConfigurationRow : IUpstreamDbEntity, M.ICrawlConfigurationRow { }
}
