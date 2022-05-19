using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for entities that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="M.ISymbolicNameRow" />
    /// <seealso cref="Upstream.Model.ISymbolicNameRow" />
    public interface ILocalSymbolicNameRow : ILocalDbEntity, M.ISymbolicNameRow { }
}
