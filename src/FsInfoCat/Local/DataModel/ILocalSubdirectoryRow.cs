namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for a database entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemRow" />
    /// <seealso cref="ISubdirectoryRow" />
    /// <seealso cref="Upstream.IUpstreamSubdirectoryRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalSubdirectoryRow")]
    public interface ILocalSubdirectoryRow : ILocalDbFsItemRow, ISubdirectoryRow { }
}
