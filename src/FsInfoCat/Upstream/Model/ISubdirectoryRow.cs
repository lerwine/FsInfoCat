using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a database entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemRow" />
    /// <seealso cref="ISubdirectoryRow" />
    /// <seealso cref="Local.Model.ISubdirectoryRow" />
    public interface IUpstreamSubdirectoryRow : IUpstreamDbFsItemRow, ISubdirectoryRow { }
}
