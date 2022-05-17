namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a structural instance of file.
    /// </summary>
    /// <seealso cref="ILocalDbFsItemRow" />
    /// <seealso cref="IFileRow" />
    /// <seealso cref="Upstream.IUpstreamFileRow" />
    public interface ILocalFileRow : ILocalDbFsItemRow, IFileRow { }
}
