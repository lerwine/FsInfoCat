namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for file system list item entities.
    /// </summary>
    /// <seealso cref="ILocalFileSystemRow" />
    /// <seealso cref="IFileSystemListItem" />
    /// <seealso cref="Upstream.IUpstreamFileSystemListItem" />
    public interface ILocalFileSystemListItem : ILocalFileSystemRow, IFileSystemListItem { }
}
