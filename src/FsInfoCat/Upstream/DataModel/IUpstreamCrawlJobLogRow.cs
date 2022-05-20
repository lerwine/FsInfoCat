namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a file system crawl job log entity.
    /// </summary>
    /// <seealso cref="ICrawlJobLogRow" />
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="Local.ILocalCrawlJobLogRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamCrawlJobLogRow")]
    public interface IUpstreamCrawlJobLogRow : ICrawlJobLogRow, IUpstreamDbEntity { }
}
