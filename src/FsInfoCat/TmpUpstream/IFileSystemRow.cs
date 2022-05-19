using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for file system entities.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="M.IFileSystemRow" />
    /// <seealso cref="Local.Model.IFileSystemRow" />
    public interface IUpstreamFileSystemRow : IUpstreamDbEntity, M.IFileSystemRow { }
}
