using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a crawl configuration entity.
    /// </summary>
    /// <seealso cref="M.ICrawlConfigurationRow" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="Upstream.Model.ICrawlConfigurationRow" />
    public interface ILocalCrawlConfigurationRow : ILocalDbEntity, M.ICrawlConfigurationRow { }
}
