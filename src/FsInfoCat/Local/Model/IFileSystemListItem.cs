using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for file system list item entities.
    /// </summary>
    /// <seealso cref="ILocalFileSystemRow" />
    /// <seealso cref="IFileSystemListItem" />
    /// <seealso cref="Upstream.Model.IFileSystemListItem" />
    public interface ILocalFileSystemListItem : ILocalFileSystemRow, IFileSystemListItem { }
}
