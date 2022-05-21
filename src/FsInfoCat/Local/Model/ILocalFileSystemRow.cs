using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for file system entities.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IFileSystemRow" />
    /// <seealso cref="Upstream.Model.IUpstreamFileSystemRow" />
    public interface ILocalFileSystemRow : ILocalDbEntity, IFileSystemRow { }
}
