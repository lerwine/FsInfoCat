namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a structural instance of file.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemRow" />
    /// <seealso cref="IFileRow" />
    /// <seealso cref="Upstream.IUpstreamFileRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalFileRow")]
    public interface ILocalFileRow : ILocalDbFsItemRow, IFileRow { }
}
