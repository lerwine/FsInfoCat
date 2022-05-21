using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a file system crawl job log entity.
    /// </summary>
    /// <seealso cref="ICrawlJobLogRow" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="Upstream.Model.ICrawlJobLogRow" />
    public interface ILocalCrawlJobLogRow : ICrawlJobLogRow, ILocalDbEntity { }
}
