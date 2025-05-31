using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for a database entity that represents a subdirectory.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamSubdirectoryRow" />
public interface ILocalSubdirectoryRow : ILocalDbFsItemRow, ISubdirectoryRow { }
