using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a file system crawl job log entity.
    /// </summary>
    /// <seealso cref="ICrawlJobLogRow" />
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="Local.Model.ILocalCrawlJobLogRow" />
    public interface IUpstreamCrawlJobLogRow : ICrawlJobLogRow, IUpstreamDbEntity { }
}
