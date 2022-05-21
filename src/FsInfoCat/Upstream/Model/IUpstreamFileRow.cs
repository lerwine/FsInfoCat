using FsInfoCat.Model;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Represents a structural instance of file.
    /// </summary>
    /// <seealso cref="IUpstreamDbFsItemRow" />
    /// <seealso cref="IFileRow" />
    /// <seealso cref="Local.Model.ILocalFileRow" />
    public interface IUpstreamFileRow : IUpstreamDbFsItemRow, IFileRow { }
}
