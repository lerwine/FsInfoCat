using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for file system entities.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamFileSystemRow" />
public interface ILocalFileSystemRow : ILocalDbEntity, IFileSystemRow { }
