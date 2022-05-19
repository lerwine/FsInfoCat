using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Interface for list item entities that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="ILocalSymbolicNameRow" />
    /// <seealso cref="M.ISymbolicNameListItem" />
    /// <seealso cref="Upstream.Model.ISymbolicNameListItem" />
    public interface ILocalSymbolicNameListItem : ILocalSymbolicNameRow, M.ISymbolicNameListItem { }
}
