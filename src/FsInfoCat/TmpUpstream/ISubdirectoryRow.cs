using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a database entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemRow" />
    /// <seealso cref="M.ISubdirectoryRow" />
    /// <seealso cref="Local.Model.ISubdirectoryRow" />
    public interface IUpstreamSubdirectoryRow : IUpstreamDbFsItemRow, M.ISubdirectoryRow { }
}
