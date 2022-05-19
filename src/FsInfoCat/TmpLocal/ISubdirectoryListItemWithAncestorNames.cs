using M = FsInfoCat.Model;
namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory and contains the names of the ancestor subdirectories.
    /// </summary>
    /// <seealso cref="M.ISubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="ILocalSubdirectoryRow" />
    /// <seealso cref="Upstream.Model.ISubdirectoryListItemWithAncestorNames" />
    public interface ILocalSubdirectoryListItemWithAncestorNames : M.ISubdirectoryListItemWithAncestorNames, ILocalSubdirectoryRow { }
}
