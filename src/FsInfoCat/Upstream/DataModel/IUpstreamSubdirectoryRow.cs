namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a database entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemRow" />
    /// <seealso cref="ISubdirectoryRow" />
    /// <seealso cref="Local.ILocalSubdirectoryRow" />
    public interface IUpstreamSubdirectoryRow : IUpstreamDbFsItemRow, ISubdirectoryRow { }
}
