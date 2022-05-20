namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for file system list item entities.
    /// </summary>
    /// <seealso cref="ILocalFileSystemRow" />
    /// <seealso cref="IFileSystemListItem" />
    /// <seealso cref="Upstream.IUpstreamFileSystemListItem" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalFileSystemListItem")]
    public interface ILocalFileSystemListItem : ILocalFileSystemRow, IFileSystemListItem { }
}
