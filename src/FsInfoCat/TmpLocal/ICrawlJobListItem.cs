using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Represents a file system crawl job log list item entity.
    /// </summary>
    /// <seealso cref="ILocalCrawlJobLogRow" />
    /// <seealso cref="M.ICrawlJobListItem" />
    /// <seealso cref="Upstream.Model.ICrawlJobListItem" />
    public interface ILocalCrawlJobListItem : ILocalCrawlJobLogRow, M.ICrawlJobListItem { }
}
