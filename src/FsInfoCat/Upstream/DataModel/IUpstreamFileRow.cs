namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Represents a structural instance of file.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemRow" />
    /// <seealso cref="IFileRow" />
    /// <seealso cref="Local.ILocalFileRow" />
    public interface IUpstreamFileRow : IUpstreamDbFsItemRow, IFileRow { }
}
