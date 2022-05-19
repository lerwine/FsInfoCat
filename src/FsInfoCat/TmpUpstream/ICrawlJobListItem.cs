using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a file system crawl job log list item entity.
    /// </summary>
    /// <seealso cref="IUpstreamCrawlJobLogRow" />
    /// <seealso cref="M.ICrawlJobListItem" />
    /// <seealso cref="Local.Model.ICrawlJobListItem" />
    public interface IUpstreamCrawlJobListItem : IUpstreamCrawlJobLogRow, M.ICrawlJobListItem { }
}
