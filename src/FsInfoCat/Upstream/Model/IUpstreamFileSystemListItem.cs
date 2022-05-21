using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for file system list item entities.
    /// </summary>
    /// <seealso cref="IUpstreamFileSystemRow" />
    /// <seealso cref="IFileSystemListItem" />
    /// <seealso cref="Local.Model.ILocalFileSystemListItem" />
    public interface IUpstreamFileSystemListItem : IUpstreamFileSystemRow, IFileSystemListItem { }
}
