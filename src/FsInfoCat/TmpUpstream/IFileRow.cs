using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a structural instance of file.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemRow" />
    /// <seealso cref="M.IFileRow" />
    /// <seealso cref="Local.Model.IFileRow" />
    public interface IUpstreamFileRow : IUpstreamDbFsItemRow, M.IFileRow { }
}
