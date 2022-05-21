using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for file system entities.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IFileSystemRow" />
    /// <seealso cref="Local.Model.ILocalFileSystemRow" />
    public interface IUpstreamFileSystemRow : IUpstreamDbEntity, IFileSystemRow { }
}
