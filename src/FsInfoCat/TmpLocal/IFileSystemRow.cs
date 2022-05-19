using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for file system entities.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="M.IFileSystemRow" />
    /// <seealso cref="Upstream.Model.IFileSystemRow" />
    public interface ILocalFileSystemRow : ILocalDbEntity, M.IFileSystemRow { }
}
