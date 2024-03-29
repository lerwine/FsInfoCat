using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a crawl configuration entity.
    /// </summary>
    /// <seealso cref="ICrawlConfigurationRow" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="Upstream.Model.IUpstreamCrawlConfigurationRow" />
    public interface ILocalCrawlConfigurationRow : ILocalDbEntity, ICrawlConfigurationRow { }
}
