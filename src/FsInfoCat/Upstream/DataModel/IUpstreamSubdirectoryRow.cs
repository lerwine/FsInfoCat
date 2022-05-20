namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a database entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemRow" />
    /// <seealso cref="ISubdirectoryRow" />
    /// <seealso cref="Local.ILocalSubdirectoryRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamSubdirectoryRow")]
    public interface IUpstreamSubdirectoryRow : IUpstreamDbFsItemRow, ISubdirectoryRow { }
}
