using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Represents a structural instance of file.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamFileRow" />
public interface ILocalFileRow : ILocalDbFsItemRow, IFileRow { }
