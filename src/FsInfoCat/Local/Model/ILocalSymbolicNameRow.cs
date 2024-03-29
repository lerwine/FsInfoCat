using FsInfoCat.Model;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ISymbolicNameRow" />
    /// <seealso cref="Upstream.Model.IUpstreamSymbolicNameRow" />
    public interface ILocalSymbolicNameRow : ILocalDbEntity, ISymbolicNameRow { }
}
