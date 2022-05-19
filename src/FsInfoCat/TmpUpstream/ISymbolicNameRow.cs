using M = FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for  that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="M.ISymbolicNameRow" />
    /// <seealso cref="Local.Model.ISymbolicNameRow" />
    public interface IUpstreamSymbolicNameRow : IUpstreamDbEntity, M.ISymbolicNameRow { }
}
