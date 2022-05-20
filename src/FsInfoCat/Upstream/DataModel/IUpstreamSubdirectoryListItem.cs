namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="ISubdirectoryListItem" />
    /// <seealso cref="IUpstreamSubdirectoryRow" />
    /// <seealso cref="Local.ILocalSubdirectoryListItem" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamSubdirectoryListItem")]
    public interface IUpstreamSubdirectoryListItem : ISubdirectoryListItem, IUpstreamSubdirectoryRow { }
}
