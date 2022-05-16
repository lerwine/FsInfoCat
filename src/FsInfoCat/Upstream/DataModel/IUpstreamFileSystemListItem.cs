namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for file system list item entities.
    /// </summary>
    /// <seealso cref="IUpstreamFileSystemRow" />
    /// <seealso cref="IFileSystemListItem" />
    /// <seealso cref="Local.ILocalFileSystemListItem" />
    public interface IUpstreamFileSystemListItem : IUpstreamFileSystemRow, IFileSystemListItem { }
}
