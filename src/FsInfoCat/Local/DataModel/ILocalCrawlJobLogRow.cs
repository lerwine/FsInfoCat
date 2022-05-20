namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a file system crawl job log entity.
    /// </summary>
    /// <seealso cref="ICrawlJobLogRow" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="Upstream.IUpstreamCrawlJobLogRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalCrawlJobLogRow")]
    public interface ILocalCrawlJobLogRow : ICrawlJobLogRow, ILocalDbEntity { }
}
