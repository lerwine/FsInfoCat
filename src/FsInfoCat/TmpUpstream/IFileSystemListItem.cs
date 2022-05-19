using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for file system list item entities.
    /// </summary>
    /// <seealso cref="IUpstreamFileSystemRow" />
    /// <seealso cref="M.IFileSystemListItem" />
    /// <seealso cref="Local.Model.IFileSystemListItem" />
    public interface IUpstreamFileSystemListItem : IUpstreamFileSystemRow, M.IFileSystemListItem { }
}
