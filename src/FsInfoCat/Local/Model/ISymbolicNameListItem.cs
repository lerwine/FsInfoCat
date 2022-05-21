using FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Interface for list item entities that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="ILocalSymbolicNameRow" />
    /// <seealso cref="ISymbolicNameListItem" />
    /// <seealso cref="Upstream.Model.ISymbolicNameListItem" />
    public interface ILocalSymbolicNameListItem : ILocalSymbolicNameRow, ISymbolicNameListItem { }
}
