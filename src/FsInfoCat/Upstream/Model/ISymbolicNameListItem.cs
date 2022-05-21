using FsInfoCat.Model;
namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Interface for list item entities that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="IUpstreamSymbolicNameRow" />
    /// <seealso cref="ISymbolicNameListItem" />
    /// <seealso cref="Local.Model.ISymbolicNameListItem" />
    public interface IUpstreamSymbolicNameListItem : IUpstreamSymbolicNameRow, ISymbolicNameListItem { }
}
